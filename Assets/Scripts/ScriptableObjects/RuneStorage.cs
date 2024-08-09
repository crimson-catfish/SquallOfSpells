using System.Collections.Generic;
using SquallOfSpells.RuneSystem;
using UnityEditor;
using UnityEngine;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Rune storage", menuName = "Scriptable objects/Rune storage")]
    public class RuneStorage : ScriptableObject
    {
        private static readonly DuplicateKeyComparer<float> Comparer = new();

        private bool areRunesSorted;

        public Dictionary<int, Rune>  Runes            { get; } = new();
        public SortedList<float, int> RunesHeight      { get; } = new(Comparer);
        public SortedList<float, int> RunesMassCenterX { get; } = new(Comparer);
        public SortedList<float, int> RunesMassCenterY { get; } = new(Comparer);

        private void OnEnable()
        {
#if UNITY_EDITOR
            EditorUtility.DisplayProgressBar("Resorting runes", "In process", 0);
#endif

            Runes.Clear();
            RunesHeight.Clear();
            RunesMassCenterX.Clear();
            RunesMassCenterY.Clear();

            foreach (Rune rune in Resources.LoadAll<Rune>("Runes"))
                Runes.Add(rune.GetHashCode(), rune);

            foreach (KeyValuePair<int, Rune> rune in Runes)
            {
                RunesHeight.Add(rune.Value.averageHeight, rune.Key);
                RunesMassCenterX.Add(rune.Value.averageMassCenter.x, rune.Key);
                RunesMassCenterY.Add(rune.Value.averageMassCenter.y, rune.Key);
            }

#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
        }


        public void AddRune(Rune rune)
        {
            Runes.Add(rune.GetHashCode(), rune);

            AddRuneProperties(rune);
        }

        public void AddVariationToRune(RuneVariation variation, Rune rune)
        {
            RemoveRuneProperties(rune);

            rune.drawVariations.Add(variation);

            AddRuneProperties(rune);
        }

        public void DeleteRune(Rune rune)
        {
            Runes.Remove(rune.GetHashCode());
            RemoveRuneProperties(rune);
        }

        private void AddRuneProperties(Rune rune)
        {
            RunesHeight.Add(rune.averageHeight, rune.GetHashCode());
            RunesMassCenterX.Add(rune.averageMassCenter.x, rune.GetHashCode());
            RunesMassCenterY.Add(rune.averageMassCenter.y, rune.GetHashCode());
        }

        private void RemoveRuneProperties(Rune rune)
        {
            RunesHeight.RemoveAt(RunesHeight.IndexOfValue(rune.GetHashCode()));
            RunesMassCenterX.RemoveAt(RunesMassCenterX.IndexOfValue(rune.GetHashCode()));
            RunesMassCenterY.RemoveAt(RunesMassCenterY.IndexOfValue(rune.GetHashCode()));
        }
    }
}