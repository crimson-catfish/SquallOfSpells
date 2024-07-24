using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuneRecognizer : MonoBehaviour
{
    [SerializeField] private RuneStorage     storage;
    [SerializeField] private RuneDrawManager drawManager;
    [SerializeField] private bool            printRecognized;


    [Header("Recognition settings")] [SerializeField]
    private float acceptableHeightDifferencePercent = 30f;

    [SerializeField] private float acceptableMassCenterDifferencePercent = 20f;
    [SerializeField] private float acceptableError                       = 0.05f;
    public event Action<Rune>      OnRuneRecognized;

    private void OnEnable()
    {
        drawManager.OnRuneDrawn += OnRuneDrawn;
    }

    private void OnDisable()
    {
        drawManager.OnRuneDrawn -= OnRuneDrawn;
    }


    private void OnRuneDrawn(RuneDrawVariation drawToCheck)
    {
        HashSet<int> selectedRuneHashes =
            new(FindClosestRunesByParams(drawToCheck.height, storage.RunesHeight,
                acceptableHeightDifferencePercent));

        selectedRuneHashes.IntersectWith(FindClosestRunesByParams(drawToCheck.massCenter.x, storage.RunesMassCenterX,
            acceptableMassCenterDifferencePercent));

        selectedRuneHashes.IntersectWith(FindClosestRunesByParams(drawToCheck.massCenter.y, storage.RunesMassCenterY,
            acceptableMassCenterDifferencePercent));

        List<Rune> runesToCheck = new();

        foreach (int hash in selectedRuneHashes)
            runesToCheck.Add(storage.Runes[hash]);

        Rune closestRune = null;
        float minError = acceptableError;

        foreach (Rune rune in runesToCheck)
        {
            float runeError = DeepCheck(drawToCheck, rune); // expensive!

            if (runeError < minError)
            {
                minError = runeError;
                closestRune = rune;
            }
        }

        OnRuneRecognized?.Invoke(closestRune);

        if (printRecognized && closestRune != null)
            Debug.Log("Recognized as " + closestRune.name);
    }

    private IEnumerable<int> FindClosestRunesByParams(
        float                  runeParam,
        SortedList<float, int> sortedRunes,
        float                  acceptableDifferencePercent)
    {
        if (sortedRunes.Count == 0)
            return Enumerable.Empty<int>();

        int lowBound = Search.Binary(sortedRunes.Keys, runeParam - (1 + acceptableDifferencePercent / 100f));
        int topBound = Search.Binary(sortedRunes.Keys, runeParam + (1 + acceptableDifferencePercent / 100f));

        return sortedRunes.Values.Skip(lowBound).Take(topBound - lowBound);
    }

    /// <summary>
    ///     Expensive compilation O(variationsCount * pointsCount^2).
    ///     Use multithreading.
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