using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SquallOfSpells.SigilSystem
{
    /// <summary>
    ///     rune's width always 1, height is positive float.
    /// </summary>
    [Serializable]
    public class Rune : ScriptableObject, IComparable<Rune>
    {
        public const float Width = 1;

        public                                          string      previewPath;
        public                                          float       averageHeight;
        public                                          Vector2     averageMassCenter = Vector2.zero;
        [FormerlySerializedAs("drawVariations")] public List<Sigil> sigils            = new();

        private Texture2D preview;

#if UNITY_EDITOR
        public Texture2D Preview {
            get
            {
                if (preview == null)
                    preview = AssetDatabase.LoadAssetAtPath<Texture2D>(previewPath);

                return preview;
            }
            set => preview = value;
        }
#endif

        public int CompareTo(Rune other)
        {
            if (sigils.GetHashCode() < other.sigils.GetHashCode()) return -1;
            if (sigils.GetHashCode() > other.sigils.GetHashCode()) return 1;

            return 0;
        }
    }
}