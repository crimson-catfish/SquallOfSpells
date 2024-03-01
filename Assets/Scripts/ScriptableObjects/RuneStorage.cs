using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;
using System.Linq;
using System;
using Unity.Mathematics;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RuneStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    [SerializeField] private int previewSize;
    [SerializeField] private int previewBorder;
    [SerializeField] private int previewPointRadius;

    [property: SerializeField] public HashSet<Rune> Runes { get; private set; } = new();


    private void OnEnable()
    {
        Runes.AddRange(Resources.LoadAll<Rune>("Runes"));
    }


    public void NewRune()
    {
        Rune rune = CreateInstance<Rune>();
        rune.previewPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Textures/Runes/Previews/preview.asset");
        AssetDatabase.CreateAsset(new Texture2D(previewSize, previewSize, TextureFormat.ARGB32, false), rune.previewPath);
        AssetDatabase.CreateAsset(rune, AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Runes/rune.asset"));
        Runes.Add(rune);
    }

    public void DeleteRune(Rune rune)
    {
        Runes.Remove(rune);
        AssetDatabase.DeleteAsset(rune.previewPath);
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rune));
    }

    public void AddDrawVariation(Rune rune)
    {
        RuneDrawVariation variation = RuneDrawManager.instance.drawVariation;

        if (variation == null)
        {
            Debug.Log("Draw something to save");
            return;
        }
        if (variation.points.Length <= 5)
        {
            Debug.Log("Too few points in rune");
            return;
        }

        rune.avaregeMass = (rune.avaregeMass * rune.drawVariations.Count + variation.points.Length) / (rune.drawVariations.Count + 1);
        rune.avaregeMassCenter = (rune.avaregeMassCenter * rune.drawVariations.Count + variation.massCenter) / (rune.drawVariations.Count + 1);
        rune.averageHeight = (rune.averageHeight * rune.drawVariations.Count + variation.height) / (rune.drawVariations.Count + 1);

        rune.drawVariations.Add(variation);

        Texture2D oldPreview = new(rune.Preview.width, rune.Preview.height, TextureFormat.ARGB32, false);
        Graphics.CopyTexture(rune.Preview, oldPreview);

        rune.Preview.Reinitialize(previewSize, (int)math.max(rune.Preview.height, variation.height * previewSize));

        rune.Preview.SetPixels(0, (rune.Preview.height - oldPreview.height) / 2, oldPreview.width, oldPreview.height,
            oldPreview.GetPixels(0, 0, oldPreview.width, oldPreview.height));

        foreach (Vector2 point in variation.points)
        {
            int x = (int)(point.x * (rune.Preview.width - previewBorder * 2)) + previewBorder - previewPointRadius;
            int y = (int)(point.y / variation.height * (rune.Preview.height - previewBorder * 2)) + previewBorder - previewPointRadius;

            rune.Preview.SetPixels(x, y, previewPointRadius, previewPointRadius, new Color[previewSize*previewSize]);
            
        }

        rune.Preview.Apply();
        EditorUtility.SetDirty(rune.Preview);

        EditorUtility.SetDirty(rune);
    }
}