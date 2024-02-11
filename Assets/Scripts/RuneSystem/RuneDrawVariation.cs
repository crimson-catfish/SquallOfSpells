using System;
using UnityEngine;

[Serializable]
public class RuneDrawVariation
{
    /// <summary>
    /// Normalized by X.
    /// </summary>
    public Vector2[] points;

    /// <summary>
    /// Normalized by X.
    /// </summary>
    public Vector2 massCenter = Vector2.zero;
    
    public float y2xRatio;
}