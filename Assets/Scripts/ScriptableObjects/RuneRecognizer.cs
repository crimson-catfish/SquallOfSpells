using System;
using System.Collections.Generic;
using System.Linq;
using SquallOfSpells.RuneSystem;
using SquallOfSpells.RuneSystem.Draw;
using UnityEngine;
using UnityEngine.Serialization;
using Logger = SquallOfSpells.Plugins.Logger;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Rune recognizer", menuName = "Scriptable objects/Rune recognizer")]
    public class RuneRecognizer : ScriptableObject
    {
        [SerializeField] private RuneStorage storage;

        [SerializeField] private bool log;

        [Header("Recognition settings")]
        [SerializeField] private float acceptableHeightDifferencePercent = 30f;
        [SerializeField]                                         private float acceptableMassCenterDifferencePercent = 20f;
        [SerializeField] private float acceptableMismatch                    = 0.05f;

        private Logger logger;

        // ReSharper disable once IdentifierTypo
        public event Action<Rune> OnRecognized;

        private void OnEnable()
        {
            logger = new Logger(this, log);
        }

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

            logger.Log(runesToCheck);

            Rune closestRune = null;
            float minMismatch = acceptableMismatch;

            foreach (Rune rune in runesToCheck)
            {
                float mismatch = DeepCheck(variationToCheck, rune); // expensive!

                logger.Log(rune + " | mismatch: " + mismatch);

                if (mismatch < minMismatch)
                {
                    minMismatch = mismatch;
                    closestRune = rune;
                }
            }


            OnRecognized?.Invoke(closestRune);

            if (log && closestRune != null)
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
            float totalMismatch = 0;

            foreach (RuneVariation variation in rune.drawVariations)
            {
                totalMismatch += GetVariationMismatch(variation, toCheck);
                totalMismatch += GetVariationMismatch(toCheck, variation);
            }

            return totalMismatch / rune.drawVariations.Count;
        }

        private static float GetVariationMismatch(RuneVariation baseVariation, RuneVariation maskVariation)
        {
            float totalMismatch = 0;

            foreach (Vector2 point in maskVariation.points)
            {
                totalMismatch += Closest.GetSqrDistance(point, baseVariation.points);
            }

            return totalMismatch / maskVariation.points.Length;
        }
    }
}