using System;
using System.Collections.Generic;
using System.Linq;
using SquallOfSpells.RuneSystem;
using Unity.Mathematics;
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
        [SerializeField] private float averageVariationPower = 1.2f;
        [SerializeField] private float averagePointPower = 1.2f;
        [FormerlySerializedAs("pointCountPower"), FormerlySerializedAs("pointCountDifferencePower"), SerializeField]
        private float sizeDiffPower = 0.2f;
        [SerializeField] private float acceptableHeightDifferencePercent     = 10f;
        [SerializeField] private float acceptableMassCenterDifferencePercent = 10f;
        [SerializeField] private float acceptableMismatch                    = 0.03f;

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

                logger.Log(rune.name + " | mismatch: " + mismatch);

                if (mismatch < minMismatch)
                {
                    minMismatch = mismatch;
                    closestRune = rune;
                }
            }

            logger.Log("minimal mismatch is " + minMismatch + " acceptable mismatch is " + acceptableMismatch);

            OnRecognized?.Invoke(closestRune);

            if (closestRune is not null)
                logger.Log("Recognized as " + closestRune.name);
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
        private float DeepCheck(RuneVariation toCheck, Rune rune)
        {
            float totalMismatch = 0;

            foreach (RuneVariation variation in rune.drawVariations)
            {
                totalMismatch += GetVariationMismatch(variation, toCheck);
                totalMismatch += GetVariationMismatch(toCheck, variation);
            }

            logger.Log("total " + rune.name + " rune mismatch: " + totalMismatch);

            return totalMismatch /
                   math.pow(rune.drawVariations.Count, averageVariationPower);
        }

        private float GetVariationMismatch(RuneVariation baseVariation, RuneVariation maskVariation)
        {
            float totalMismatch = 0;

            foreach (Vector2 basePoint in baseVariation.points)
            {
                float minSqrDistance = Mathf.Infinity;

                foreach (Vector2 maskPoint in maskVariation.points)
                {
                    float sqrDistance = (maskPoint - basePoint).sqrMagnitude;

                    if (sqrDistance < minSqrDistance)
                        minSqrDistance = sqrDistance;
                }

                totalMismatch += minSqrDistance;
            }

            return totalMismatch /
                   math.pow(maskVariation.points.Length, averagePointPower) /
                   math.pow((float)maskVariation.points.Length / baseVariation.points.Length, sizeDiffPower);
        }
    }
}