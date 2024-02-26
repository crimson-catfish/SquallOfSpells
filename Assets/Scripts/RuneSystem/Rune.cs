using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;

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
            if (!string.IsNullOrEmpty(_previewPath) || File.Exists(_previewPath))
            {
                _preview = AssetDatabase.LoadAssetAtPath<Texture2D>(_previewPath);
            }
            else
            {
                _preview = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Textures/Runes/Preview/DefaultPreview.png");
            }

            return _preview;
        }
    }

    private Texture2D _preview;
    [SerializeField] private string _previewPath;


    public void FreePreviewTextureFromAssets()
    {
        if (!string.IsNullOrEmpty(_previewPath) || File.Exists(_previewPath)) AssetDatabase.DeleteAsset(_previewPath);
    }


    public void AddNewRuneDrawVariation(RuneDrawVariation drawVariation)
    {
        AvaregeMass = (AvaregeMass * DrawVariations.Count + drawVariation.points.Length) / (DrawVariations.Count + 1);
        AvaregeMassCenter = (AvaregeMassCenter * DrawVariations.Count + drawVariation.massCenter) / (DrawVariations.Count + 1);
        Height = (Height * DrawVariations.Count + drawVariation.height) / (DrawVariations.Count + 1);

        DrawVariations.Add(drawVariation);

        foreach (Vector2 point in drawVariation.points)
        {
            Debug.Log(_preview.isReadable);
            _preview.SetPixel((int)(point.x * Preview.width), (int)(point.y * Preview.height), Color.black);
        }
    }
}