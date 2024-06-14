using System.Collections.Generic;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// rune's width always 1, height is positive float.
/// </summary>
[Serializable]
public class Rune : ScriptableObject, IComparable<Rune>
{
    public const float Width = 1;

    public string previewPath;
    private Texture2D preview;

#if UNITY_EDITOR
    [property: SerializeField]
    public Texture2D Preview
    {
        get
        {
            if (preview == null)
                preview = AssetDatabase.LoadAssetAtPath<Texture2D>(previewPath);

            return preview;
        }
        set => preview = value;
    }
#endif

    public float averageHeight;
    public Vector2 averageMassCenter = Vector2.zero;
    public List<RuneDrawVariation> drawVariations = new();

    public int CompareTo(Rune other)
    {
        if (this.GetHashCode() < other.GetHashCode()) return -1;
        if (this.GetHashCode() > other.GetHashCode()) return 1;
        return 0;
    }
}