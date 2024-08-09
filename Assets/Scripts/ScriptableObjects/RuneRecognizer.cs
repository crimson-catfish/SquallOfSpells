using System;
using System.Collections.Generic;
using System.Linq;
using SquallOfSpells.RuneSystem;
using SquallOfSpells.RuneSystem.Draw;
using UnityEngine;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Rune recognizer", menuName = "Scriptable objects/Rune recognizer")]
    public class RuneRecognizer : ScriptableObject
    {
        [SerializeField] private RuneStorage storage;
        [SerializeField] private bool        printRecognized;

        [Header("Recognition settings")]
        [SerializeField] private float acceptableHeightDifferencePercent = 30f;
        [SerializeField] private float acceptableMassCenterDifferencePercent = 20f;
        [SerializeField] private float acceptableError                       = 0.05f;

        // ReSharper disable once IdentifierTypo
        public event Action<Rune> OnRecognized;


        public void Recognize(RuneVariation variationToCheck)
        {
            HashSet<int> selectedRuneHashes =
                new(FindClosestRunesByParams(variationToCheck.height, storage.RunesHeight,
                    acceptableHeightDifferencePercent));

            selectedRuneHashes.IntersectWith(FindClosestRunesByParams(variationToCheck.massCenter.x,
                storage.RunesMassCenterX,
                acceptableMassCenterDifferencePercent));

            selectedRuneHashes.IntersectWith(FindClosestRunesByParams(variationToCheck.massCenter.y,
                storage.RunesMassCenterY,
                acceptableMassCenterDifferencePercent));

            List<Rune> runesToCheck = new();

            foreach (int hash in selectedRuneHashes)
                runesToCheck.Add(storage.Runes[hash]);

            Rune closestRune = null;
            float minError = acceptableError;

            foreach (Rune rune in runesToCheck)
            {
                float runeError = DeepCheck(variationToCheck, rune); // expensive!

                if (runeError < minError)
                {
                    minError = runeError;
                    closestRune = rune;
                }
            }

            OnRecognized?.Invoke(closestRune);

            if (printRecognized && closestRune != null)
                Debug.Log("Recognized as " + closestRune.name);
        }

        private static IEnumerable<int> FindClosestRunesByParams(
            float runeParam, SortedList<float, int> sortedRunes, float acceptableDifferencePercent)
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
        private static float DeepCheck(RuneVariation toCheck, Rune rune)
        {
            float totalRuneError = 0;

            foreach (RuneVariation variation in rune.drawVariations)
            {
                totalRuneError += GetAverageVariationError(variation, toCheck);
                totalRuneError += GetAverageVariationError(toCheck, variation);
            }

            return totalRuneError / rune.drawVariations.Count;
        }

        private static float GetAverageVariationError(RuneVariation baseVariation, RuneVariation maskVariation)
        {
            float totalVariationError = 0;

            foreach (Vector2 point in maskVariation.points)
            {
                totalVariationError += Closest.GetSqrDistance(point, baseVariation.points);
            }

            return totalVariationError / maskVariation.points.Length;
        }
    }
}