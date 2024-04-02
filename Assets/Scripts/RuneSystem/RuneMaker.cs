using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class RuneMaker : Singleton<RuneMaker>
{
    [SerializeField] private RuneStorage storage;
    [SerializeField] private RunePreviewParameters param;

    private Color[] pointColors;


    private void OnEnable()
    {
        pointColors = new Color[param.pointRadius * param.pointRadius];
        for (int i = 0; i < param.pointRadius * param.pointRadius; i++) pointColors[i] = param.pointColor;
    }


    public Rune SaveDrawVariationToNewRune()
    {
        Rune rune = ScriptableObject.CreateInstance<Rune>();
        rune.previewPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Textures/Runes/Previews/preview.asset");
        AssetDatabase.CreateAsset(new Texture2D(param.size, param.size, TextureFormat.ARGB32, false), rune.previewPath);
        Debug.Log(new Texture2D(34, 34));
        AssetDatabase.CreateAsset(rune, AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Runes/rune.asset"));
        storage.runes.Add(rune.GetHashCode(), rune);

        AddDrawVariation(rune);
        return rune;
    }

    public void DeleteRune(Rune rune)
    {
        storage.runes.Remove(rune.GetHashCode());
        AssetDatabase.DeleteAsset(rune.previewPath);
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rune));
        storage.areSortedListsUpdated = false;
    }

    public void AddDrawVariation(Rune rune)
    {
        Undo.RegisterCompleteObjectUndo(rune, "added draw variation to some rune");
        Undo.RegisterCompleteObjectUndo(rune.Preview, "changed preview image of added rune");

        RuneDrawVariation variation = RuneDrawManager.instance.drawVariation;

        if (variation == null)
        {
            Debug.LogWarning("Draw something to save");
            return;
        }
        if (variation.points.Length <= 5)
        {
            Debug.LogWarning("Too few points in rune, didn't save");
            return;
        }
        if (rune.drawVariations.Contains(variation)) return;

        // update rune data
        rune.avaregeMassCenter = (rune.avaregeMassCenter * rune.drawVariations.Count + variation.massCenter) / (rune.drawVariations.Count + 1);
        rune.averageHeight = (rune.averageHeight * rune.drawVariations.Count + variation.height) / (rune.drawVariations.Count + 1);
        if (!rune.drawVariations.Contains(variation)) rune.drawVariations.Add(variation);

        // resize preview texture
        Texture2D oldPreview = new(rune.Preview.width, rune.Preview.height, TextureFormat.ARGB32, false);
        Graphics.CopyTexture(rune.Preview, oldPreview);
        rune.Preview.Reinitialize(param.size, (int)math.max(rune.Preview.height, variation.height * param.size));
        rune.Preview.SetPixels(0, (rune.Preview.height - oldPreview.height) / 2, oldPreview.width, oldPreview.height,
            oldPreview.GetPixels(0, 0, oldPreview.width, oldPreview.height));


        // add new draw variation on preview texture
        foreach (Vector2 point in variation.points)
        {
            int x = (int)(point.x * (rune.Preview.width - param.border * 2)) + param.border - param.pointRadius;
            int y = (int)(point.y / variation.height * (rune.Preview.height - param.border * 2)) + param.border - param.pointRadius;

            Color[] colors = rune.Preview.GetPixels(x, y, param.pointRadius, param.pointRadius);
            for (int i = 1; i < colors.Length; ++i) colors[i] += param.pointColor;
            rune.Preview.SetPixels(x, y, param.pointRadius, param.pointRadius, colors);

        }

        // save
        rune.Preview.Apply();
        EditorUtility.SetDirty(rune.Preview);
        EditorUtility.SetDirty(rune);

        storage.areSortedListsUpdated = false;
    }

    public void ResortRunes()
    {
        storage.runesHeight = new();
        storage.runesMassCenterX = new();
        storage.runesMassCenterY = new();

        foreach (KeyValuePair<int, Rune> rune in storage.runes)
        {
            storage.runesHeight.Add(rune.Value.averageHeight, rune.Key);
            storage.runesMassCenterX.Add(rune.Value.avaregeMassCenter.x, rune.Key);
            storage.runesMassCenterY.Add(rune.Value.avaregeMassCenter.y, rune.Key);
        }

        File.WriteAllText("Assets/Resources/Runes/height.json", JsonConvert.SerializeObject(storage.runesHeight));
        File.WriteAllText("Assets/Resources/Runes/massCenterX.json", JsonConvert.SerializeObject(storage.runesMassCenterX));
        File.WriteAllText("Assets/Resources/Runes/massCenterY.json", JsonConvert.SerializeObject(storage.runesMassCenterY));

        storage.areSortedListsUpdated = true;
    }
}