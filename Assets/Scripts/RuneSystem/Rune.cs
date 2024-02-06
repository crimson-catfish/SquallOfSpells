using UnityEngine;
using System.Collections.Generic;

public class Rune
{
    private static Texture2D defaultTexture = Resources.Load<Texture2D>("Textures/Runes/defaultRunePreview");

    public Texture2D               Preview           { get; private set; } = defaultTexture;
    public float                   AvaregeMass       { get; private set; } = 0;
    public Vector2                 AvaregeMassCenter { get; private set; } = Vector2.zero;
    public float                   AvaregeRatio      { get; private set; } = 0;
    public List<RuneDrawVariation> DrawVariations    { get; private set; } = new();

    public void AddNewRuneDrawVariation(RuneDrawVariation drawVariation)
    {
        AvaregeMass       = (AvaregeMass       * DrawVariations.Count + drawVariation.mass)       / (DrawVariations.Count + 1);
        AvaregeMassCenter = (AvaregeMassCenter * DrawVariations.Count + drawVariation.massCenter) / (DrawVariations.Count + 1);
        AvaregeRatio      = (AvaregeRatio      * DrawVariations.Count + drawVariation.ratio)      / (DrawVariations.Count + 1);

        DrawVariations.Add(drawVariation);
    }
}