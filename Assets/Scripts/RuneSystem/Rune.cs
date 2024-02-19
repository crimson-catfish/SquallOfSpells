using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;
using Unity.VisualScripting;

/// <summary>
/// rune's width always 1, height is positive.
/// </summary>
[Serializable]
public class Rune
{
    public const float width = 1;

    public float AvaregeMass { get; private set; } = 0;
    public Vector2 AvaregeMassCenter { get; private set; } = Vector2.zero;
    public float Height { get; private set; } = 0;
    [field: SerializeField] public List<RuneDrawVariation> DrawVariations { get; private set; } = new();

    public Texture2D Preview
    {
        get
        {
            if (preview == null) preview = Resources.Load<Texture2D>(previewPath.PartAfter('/').PartAfter('/').PartBefore('.'));
            return preview;
        }
    }
    private Texture2D preview;
    private string previewPath;


    public Rune()
    {
        Texture2D preview = new Texture2D(128, 128, TextureFormat.ARGB32, false);
        previewPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Textures/Runes/Preview/preview.png");
        File.WriteAllBytes(previewPath, preview.EncodeToPNG());
    }

    public void FreePreviewTextureFromAssets()
    {
        AssetDatabase.DeleteAsset(previewPath);
    }


    public void AddNewRuneDrawVariation(RuneDrawVariation drawVariation)
    {
        AvaregeMass = (AvaregeMass * DrawVariations.Count + drawVariation.points.Length) / (DrawVariations.Count + 1);
        AvaregeMassCenter = (AvaregeMassCenter * DrawVariations.Count + drawVariation.massCenter) / (DrawVariations.Count + 1);
        Height = (Height * DrawVariations.Count + drawVariation.height) / (DrawVariations.Count + 1);

        DrawVariations.Add(drawVariation);

        foreach (Vector2 point in drawVariation.points)
        {
            // preview.SetPixels();
        }
    }
}