using UnityEngine;
using System.Collections.Generic;
using System;
    
/// <summary>
/// rune's width always 1, height is positive.
/// </summary>
[Serializable]
public class Rune
{
    public const float width = 1;

    [field: SerializeField] public Texture2D Preview { get; private set; }
    public float AvaregeMass { get; private set; } = 0;
    public Vector2 AvaregeMassCenter { get; private set; } = Vector2.zero;
    public float Height { get; private set; } = 0;
    [field: SerializeField] public List<RuneDrawVariation> DrawVariations { get; private set; } = new();

    public Rune(Texture2D preview)
    {
        Preview = preview;
    }

    public void AddNewRuneDrawVariation(RuneDrawVariation drawVariation)
    {
        AvaregeMass = (AvaregeMass * DrawVariations.Count + drawVariation.points.Length) / (DrawVariations.Count + 1);
        AvaregeMassCenter = (AvaregeMassCenter * DrawVariations.Count + drawVariation.massCenter) / (DrawVariations.Count + 1);
        Height = (Height * DrawVariations.Count + drawVariation.height) / (DrawVariations.Count + 1);

        DrawVariations.Add(drawVariation);

        Texture2D _preview = Preview;
        foreach (Vector2 point in drawVariation.points)
        {
            // preview.SetPixels();
        }
    }
}