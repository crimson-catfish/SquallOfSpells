using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RuneRecognizer : MonoBehaviour
{
    [SerializeField] private RuneStorage storage;

    [Header("Recognition settings")]
    [SerializeField] private float heightRange;
    [SerializeField] private float massCenterRange;

    private Image recognizedRuneRenderer;

    private RuneDrawManager drawManager;

    private void Awake()
    {
        drawManager = RuneDrawManager.instance;
        recognizedRuneRenderer = GetComponent<Image>();
    }

    private void OnEnable()
    {
        drawManager.RuneDrawn += OnRuneDrawn;
    }

    private void OnDisable()
    {
        drawManager.RuneDrawn -= OnRuneDrawn;
    }

    private void OnRuneDrawn(RuneDrawVariation runeDrawToCheck)
    {
        #if UNITY_EDITOR
            if (!storage.areSortedListsUpdated) RuneMaker.instance.ResortRunes();
        #endif
                
        HashSet<int> selectedRuneHashes = new HashSet<int>(FindClosestRunesByParams(runeDrawToCheck.height, storage.runesHeight, heightRange));
        selectedRuneHashes.IntersectWith(FindClosestRunesByParams(runeDrawToCheck.massCenter.x, storage.runesMassCenterX, massCenterRange));
        selectedRuneHashes.IntersectWith(FindClosestRunesByParams(runeDrawToCheck.massCenter.y, storage.runesMassCenterY, massCenterRange));

        List<Rune> runesToCheck = new();
        foreach (int hash in selectedRuneHashes) runesToCheck.Add(storage.runes[hash]);

        Rune closestRune = null;
        float minError = Mathf.Infinity;
        foreach (Rune rune in runesToCheck)
        {
            float runeError = DeepCheck(runeDrawToCheck, rune); // expensive!
            if (runeError < minError)
            {
                minError = runeError;
                closestRune = rune;
            }
        }


        if (closestRune != null)
        {        
            #if UNITY_EDITOR
                var tex = closestRune.Preview;
                if (tex != null) recognizedRuneRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            #endif
        }
    }

    private IEnumerable<int> FindClosestRunesByParams(float runeParam, SortedList<float, int> sortedRunes, float range)
    {
        int lowBound = Search.Binary(sortedRunes.Keys, runeParam - range);
        int topBound = Search.Binary(sortedRunes.Keys, runeParam + range);

        return sortedRunes.Values.Skip(lowBound).Take(topBound - lowBound);
    }

    /// <summary>
    /// Expensive compilation O(variationsCount * pointsCount^2).
    /// Use multithreading.
    /// </summary>
    /// <returns></returns>
    private float DeepCheck(RuneDrawVariation drawToCheck, Rune rune)
    {
        float totalRuneError = 0;

        foreach (RuneDrawVariation variation in rune.drawVariations)
        {
            totalRuneError += GetAverageVariationError(variation, drawToCheck);
            totalRuneError += GetAverageVariationError(drawToCheck, variation);
        }

        return totalRuneError / rune.drawVariations.Count;
    }

    private float GetAverageVariationError(RuneDrawVariation baseVariation, RuneDrawVariation maskVariation)
    {
        float totalVariationError = 0;
        foreach (Vector2 point in maskVariation.points)
        {
            totalVariationError += Closest.GetSqrDistance(point, baseVariation.points);
        }
        return totalVariationError / maskVariation.points.Length;
    }
}
