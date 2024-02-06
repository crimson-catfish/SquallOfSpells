using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Rune
{
    public Texture2D               Preview           { get; private set; }
    public float                   AvaregeMass       { get; private set; }
    public Vector2                 AvaregeMassCenter { get; private set; }
    public float                   AvaregeRatio      { get; private set; }
    public List<RuneDrawVariation> DrawVariations    { get; private set; }

    public void AddNewRuneDrawVariation(RuneDrawVariation drawVariation)
    {
        //TODO: set preview

        AvaregeMass       = (AvaregeMass       * DrawVariations.Count + drawVariation.mass)       / (DrawVariations.Count + 1);
        AvaregeMassCenter = (AvaregeMassCenter * DrawVariations.Count + drawVariation.massCenter) / (DrawVariations.Count + 1);
        AvaregeRatio      = (AvaregeRatio      * DrawVariations.Count + drawVariation.ratio)      / (DrawVariations.Count + 1);

        DrawVariations.Add(drawVariation);
    }
}

[Serializable]
public class RuneDrawVariation
{
    public Vector2[] points;
    public int mass = 0;
    public Vector2 massCenter;
    public float ratio;
}