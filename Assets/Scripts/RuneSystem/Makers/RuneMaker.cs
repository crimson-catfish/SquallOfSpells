using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;

public class RuneMaker : MonoBehaviour
{
    [SerializeField] private RuneStorage storage;
    [SerializeField] private RuneDrawManager drawManager;

    [Header("changing this properties doesn't affects already created previews\nPlease recreate them to apply changes")]
    [SerializeField] private int width = 128;
    [SerializeField] private int border = 8;
    [SerializeField] private int pointRadius = 4;
    [SerializeField, Range(0, 1)] private float pointDarkness = 0.3f;
    [SerializeField] private TextureFormat textureFormat;

    private bool areRunesSorted;


    private void OnDisable()
    {
        ResortRunes();
    }

    public void SaveDrawVariationToNewRune()
    {
        RuneDrawVariation variation = drawManager.drawVariation;
        if (!DoesVariationHasEnoughPoints(variation)) return;

        Rune rune = ScriptableObject.CreateInstance<Rune>();
        rune.previewPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Textures/Runes/Previews/preview.asset");
        AssetDatabase.CreateAsset(rune, AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Runes/rune.asset"));
        AssetDatabase.CreateAsset(new Texture2D(width, width, TextureFormat.ARGB32, false), rune.previewPath);
        storage.runes.Add(rune.GetHashCode(), rune);

        AddDrawVariation(rune, variation);
        areRunesSorted = false;
    }

    public void DeleteRune(Rune rune)
    {
        storage.runes.Remove(rune.GetHashCode());
        AssetDatabase.DeleteAsset(rune.previewPath);
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rune));
        areRunesSorted = false;
    }

    private bool DoesVariationHasEnoughPoints(RuneDrawVariation variation)
    {
        if (variation == null)
        {
            Debug.LogWarning("Draw something to save");
            return false;
        }

        if (variation.points.Length <= 5)
        {
            Debug.LogWarning("Too few points in rune, didn't save");
            return false;
        }

        return true;
    }

    public void AddDrawVariation(Rune rune, RuneDrawVariation variation)
    {
        if (!DoesVariationHasEnoughPoints(variation)) return;
        if (rune.drawVariations.Contains(variation)) return;

        // update rune data
        rune.averageMassCenter = (rune.averageMassCenter * rune.drawVariations.Count + variation.massCenter) /
                                 (rune.drawVariations.Count + 1);
        rune.averageHeight = (rune.averageHeight * rune.drawVariations.Count + variation.height) /
                             (rune.drawVariations.Count + 1);
        if (!rune.drawVariations.Contains(variation)) rune.drawVariations.Add(variation);

        // resize preview texture
        Texture2D tex = rune.Preview;
        Texture2D oldPreview = new(tex.width, tex.height, TextureFormat.ARGB32, false);
        Graphics.CopyTexture(tex, oldPreview);
        tex.Reinitialize(width, (int)math.max(tex.height, variation.height * width));
        tex.SetPixels(0, (tex.height - oldPreview.height) / 2, oldPreview.width, oldPreview.height,
            oldPreview.GetPixels(0, 0, oldPreview.width, oldPreview.height));


        // add new draw variation on preview texture
        foreach (Vector2 point in variation.points)
        {
            int x = (int)(point.x * (tex.width - border * 2)) + border - pointRadius;
            int y = (int)(point.y / variation.height * (tex.height - border * 2)) + border - pointRadius;

            Color[] colors = tex.GetPixels(x, y, pointRadius, pointRadius);
            for (int i = 1; i < colors.Length; ++i)
                colors[i] *= 1 - pointDarkness;
            tex.SetPixels(x, y, pointRadius, pointRadius, colors);
        }

        // save
        rune.Preview.Apply();
        EditorUtility.SetDirty(rune.Preview);
        EditorUtility.SetDirty(rune);

        areRunesSorted = false;
    }

    public void ResortRunes()
    {
        if (areRunesSorted) return;

        EditorUtility.DisplayProgressBar("Resorting runes", "In process", 0);

        storage.runesHeight.Clear();
        storage.runesMassCenterX.Clear();
        storage.runesMassCenterY.Clear();

        foreach (KeyValuePair<int, Rune> rune in storage.runes)
        {
            storage.runesHeight.Add(rune.Value.averageHeight, rune.Key);
            storage.runesMassCenterX.Add(rune.Value.averageMassCenter.x, rune.Key);
            storage.runesMassCenterY.Add(rune.Value.averageMassCenter.y, rune.Key);
        }

        File.WriteAllText("Assets/Resources/Runes/height.json", JsonConvert.SerializeObject(storage.runesHeight));
        File.WriteAllText("Assets/Resources/Runes/massCenterX.json",
            JsonConvert.SerializeObject(storage.runesMassCenterX));
        File.WriteAllText("Assets/Resources/Runes/massCenterY.json",
            JsonConvert.SerializeObject(storage.runesMassCenterY));

        areRunesSorted = true;

        EditorUtility.ClearProgressBar();
    }
}