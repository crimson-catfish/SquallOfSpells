using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;

/// <summary>
/// rune's width always 1, height is positive.
/// </summary>
[Serializable]
public class Rune : ScriptableObject, IComparable<Rune>
{
    public const float width = 1;

    public string previewPath;
    private Texture2D preview;

    [property: SerializeField]
    public Texture2D Preview
    {
        get
        {
            if (preview == null) preview = AssetDatabase.LoadAssetAtPath<Texture2D>(previewPath);
            return preview;
        }
        set { preview = value; }
    }

    public float averageHeight = 0;
    public float avaregeMass = 0;
    public Vector2 avaregeMassCenter = Vector2.zero;
    public List<RuneDrawVariation> drawVariations = new();

    public int CompareTo(Rune other)
    {
        if (this.GetHashCode() < other.GetHashCode()) return -1;
        if (this.GetHashCode() > other.GetHashCode()) return 1;
        return 0;
    }
}