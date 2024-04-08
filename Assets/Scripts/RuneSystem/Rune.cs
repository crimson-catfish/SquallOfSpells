using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
    using UnityEditor;
#endif

/// <summary>
/// rune's width always 1, height is positive.
/// </summary>
[Serializable]
public class Rune : ScriptableObject, IComparable<Rune>
{
    public const float Width = 1;

    public string previewPath;
    private Sprite preview;

    #if UNITY_EDITOR
        [property: SerializeField]
        public Sprite Preview
        {
            get
            {
                if (preview == null) preview = AssetDatabase.LoadAssetAtPath<Sprite>(previewPath);
                return preview;
            }
            set => preview = value;
        }
    #endif

    public float averageHeight = 0;
    public Vector2 averageMassCenter = Vector2.zero;
    public List<RuneDrawVariation> drawVariations = new();

    public int CompareTo(Rune other)
    {
        if (this.GetHashCode() < other.GetHashCode()) return -1;
        if (this.GetHashCode() > other.GetHashCode()) return 1;
        return 0;
    }
}