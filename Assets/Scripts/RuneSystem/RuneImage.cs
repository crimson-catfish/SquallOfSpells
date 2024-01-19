using System;
using UnityEngine;

[Serializable]
public class RuneImage
{
    public const int SIZE_IN_PIXELS = 512;
    public const int BYTES_PER_PIXEL = 1;

    public string name;
    public byte[,] image;
    public int mass;
    public Vector2 massCenter;
    
    public RuneImage(string nameToSet)
    {
        name = nameToSet;
        image = new byte[SIZE_IN_PIXELS, SIZE_IN_PIXELS];
        mass = 0;
    }
}