using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuneRecognizer : MonoBehaviour
{
    [SerializeField] private RuneStorage storage;

    [Header("Recognition settings")]
    [SerializeField] private readonly float heightRange;
    [SerializeField] private readonly float massCenterRange;
    [SerializeField] private readonly float massRange;


    private RuneDrawManager drawManager;

    private void Awake()
    {
        drawManager = RuneDrawManager.instance;
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

        foreach (Rune rune in runesToCheck)
        {
            DeepCheck(runeDrawToCheck, rune);
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
    private Errors DeepCheck(RuneDrawVariation drawVariationToCheck, Rune rune)
    {
        Errors errors = new() { totalError = 0, minError = Mathf.Infinity };
        foreach (RuneDrawVariation variation in rune.drawVariations)
        {
            float error = 0;
            foreach (Vector2 point in variation.points)
            {
                error += Closest.GetSqrDistance(point, drawVariationToCheck.points);
            }
            errors.totalError += error;
            if (error < errors.minError) errors.minError = error;
        }

        return errors;
    }

    private struct Errors
    {
        public float totalError;
        public float minError;
    }
}
