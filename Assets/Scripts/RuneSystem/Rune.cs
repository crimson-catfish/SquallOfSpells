using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Rune
{
    public Texture2D preview;
    public float avaregeMass;
    public Vector2 avaregeMassCenter;
    public float avaregeRatio;
    public List<RuneVariation> drawVariations;
}

[Serializable]
public class RuneVariation
{
    public Vector2[] points;
    public int mass = 0;
    public Vector2 massCenter;
    public float ratio;
}