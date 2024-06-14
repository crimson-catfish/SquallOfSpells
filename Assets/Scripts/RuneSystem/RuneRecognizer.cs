using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RuneRecognizer : MonoBehaviour
{
    public Action<Rune> OnRuneRecognized;
    
    [SerializeField] private RuneStorage storage;
    [SerializeField] private RuneDrawManager drawManager;

    [Header("Recognition settings")]
    [SerializeField] private float heightRange;
    [SerializeField] private float massCenterRange;

    private Image recognizedRuneRenderer;
    
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
        HashSet<int> selectedRuneHashes = new HashSet<int>(FindClosestRunesByParams(drawToCheck.height, storage.RunesHeight, heightRange));
        selectedRuneHashes.IntersectWith(FindClosestRunesByParams(drawToCheck.massCenter.x, storage.RunesMassCenterX, massCenterRange));
        selectedRuneHashes.IntersectWith(FindClosestRunesByParams(drawToCheck.massCenter.y, storage.RunesMassCenterY, massCenterRange));

        List<Rune> runesToCheck = new();
        foreach (int hash in selectedRuneHashes) runesToCheck.Add(storage.Runes[hash]);

        Rune closestRune = null;
        float minError = Mathf.Infinity;
        foreach (Rune rune in runesToCheck)
        {
            float runeError = DeepCheck(drawToCheck, rune); // expensive!
            if (runeError < minError)
            {
                minError = runeError;
                closestRune = rune;
            }
        }


        if (closestRune != null)
        {        
            OnRuneRecognized?.Invoke(closestRune);
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
