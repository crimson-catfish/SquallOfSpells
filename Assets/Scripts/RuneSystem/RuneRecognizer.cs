using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RuneRecognizer : MonoBehaviour
{
    [SerializeField] private RuneStorage storage;

    [Header("Recognition settings")]
    [SerializeField] private float heightRange;
    [SerializeField] private float massCenterRange;
    [SerializeField] private float massRange;

    private SpriteRenderer renderer;

    private RuneDrawManager drawManager;

    private void Awake()
    {
        drawManager = RuneDrawManager.instance;
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        drawManager.RuneDrawn += (runeDraw) => OnRuneDrawn(runeDraw);
    }

    private void OnDisable()
    {
        drawManager.RuneDrawn -= (runeDraw) => OnRuneDrawn(runeDraw);
    }

    private void OnRuneDrawn(RuneDrawVariation runeDrawToCheck)
    {
        HashSet<int> selectedRuneHashes = new HashSet<int>(FindClosestRunesByParams(runeDrawToCheck.height, storage.runesHeight, heightRange));
        selectedRuneHashes.IntersectWith(FindClosestRunesByParams(runeDrawToCheck.massCenter.x, storage.runesMassCenterX, massCenterRange));
        selectedRuneHashes.IntersectWith(FindClosestRunesByParams(runeDrawToCheck.massCenter.y, storage.runesMassCenterY, massCenterRange));
        selectedRuneHashes.IntersectWith(FindClosestRunesByParams(runeDrawToCheck.points.Length, storage.runesMass, massRange));

        List<Rune> runesToCheck = new();
        foreach (int hash in selectedRuneHashes) runesToCheck.Add(storage.runes[hash]);

        foreach (var v in runesToCheck) Debug.Log(v);

        CheckResult bestCheckResult = new() { totalError = Mathf.Infinity, minError = Mathf.Infinity };
        Rune runeWithBestVariation;
        Rune runeWithBestTotal = null;
        foreach (Rune rune in runesToCheck)
        {
            CheckResult checkResult = DeepCheck(runeDrawToCheck, rune); // expencive!

            if (checkResult.totalError < bestCheckResult.totalError)
            {
                bestCheckResult.totalError = checkResult.totalError;
                runeWithBestTotal = rune;
            }
            if (checkResult.minError < bestCheckResult.minError)
            {
                bestCheckResult.minError = checkResult.minError;
                runeWithBestVariation = rune;
            }
        }

        if (runeWithBestTotal != null)
        {
            var tex = runeWithBestTotal.Preview;
            if (tex != null) renderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
    }

    private IEnumerable<int> FindClosestRunesByParams(float runeParam, SortedList<float, int> sortedRunes, float range)
    {
        int lowBound = Search.Binary<float>(sortedRunes.Keys, runeParam - range);
        int topBound = Search.Binary<float>(sortedRunes.Keys, runeParam + range);

        return sortedRunes.Values.Skip(lowBound).Take(topBound - lowBound);
    }

    /// <summary>
    /// Expensive comparation O(variationsCount * pointsCount^2).
    /// Use multithreading.
    /// </summary>
    /// <returns></returns>
    private CheckResult DeepCheck(RuneDrawVariation drawVariationToCheck, Rune rune)
    {
        CheckResult errors = new() { totalError = 0, minError = Mathf.Infinity };
        foreach (RuneDrawVariation variation in rune.drawVariations)
        {
            float error = 0;

            foreach (Vector2 point in variation.points)
            {
                error += Closest.GetSqrDistance(point, drawVariationToCheck.points);
            }

            foreach (Vector2 point in drawVariationToCheck.points)
            {
                error += Closest.GetSqrDistance(point, variation.points);
            }

            errors.totalError += error;
            if (error < errors.minError)
            {
                errors.minError = error;
                errors.bestDrawVariation = variation;
            }
        }

        return errors;
    }

    private struct CheckResult
    {
        public float totalError;
        public float minError;
        public RuneDrawVariation bestDrawVariation;
    }
}
