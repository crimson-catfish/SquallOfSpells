using System.IO;
using System.Threading;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class RuneMaker : Singleton<RuneMaker>
{
    [SerializeField] private RuneStorage storage;
    [SerializeField] private RuneMakerParameters prms;


    public void NewRune()
    {
        Rune rune = ScriptableObject.CreateInstance<Rune>();
        rune.previewPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Textures/Runes/Previews/preview.asset");
        AssetDatabase.CreateAsset(new Texture2D(prms.previewSize, prms.previewSize, TextureFormat.ARGB32, false), rune.previewPath);
        Debug.Log(new Texture2D(34, 34));
        AssetDatabase.CreateAsset(rune, AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Runes/rune.asset"));
        storage.runes.Add(rune);
    }

    public void DeleteRune(Rune rune)
    {
        storage.runes.Remove(rune);
        AssetDatabase.DeleteAsset(rune.previewPath);
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rune));
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

        // update rune data
        rune.avaregeMass = (rune.avaregeMass * rune.drawVariations.Count + variation.points.Length) / (rune.drawVariations.Count + 1);
        rune.avaregeMassCenter = (rune.avaregeMassCenter * rune.drawVariations.Count + variation.massCenter) / (rune.drawVariations.Count + 1);
        rune.averageHeight = (rune.averageHeight * rune.drawVariations.Count + variation.height) / (rune.drawVariations.Count + 1);
        if (!rune.drawVariations.Contains(variation)) rune.drawVariations.Add(variation);

        // resize preview texture
        Texture2D oldPreview = new(rune.Preview.width, rune.Preview.height, TextureFormat.ARGB32, false);
        Graphics.CopyTexture(rune.Preview, oldPreview);
        rune.Preview.Reinitialize(prms.previewSize, (int)math.max(rune.Preview.height, variation.height * prms.previewSize));
        rune.Preview.SetPixels(0, (rune.Preview.height - oldPreview.height) / 2, oldPreview.width, oldPreview.height,
            oldPreview.GetPixels(0, 0, oldPreview.width, oldPreview.height));

        // add new draw variation on preview texture
        foreach (Vector2 point in variation.points)
        {
            int x = (int)(point.x * (rune.Preview.width - prms.previewBorder * 2)) + prms.previewBorder - prms.previewPointRadius;
            int y = (int)(point.y / variation.height * (rune.Preview.height - prms.previewBorder * 2)) + prms.previewBorder - prms.previewPointRadius;

            rune.Preview.SetPixels(x, y, prms.previewPointRadius, prms.previewPointRadius, new Color[prms.previewSize*prms.previewSize]);
        }

        // save
        rune.Preview.Apply();
        EditorUtility.SetDirty(rune.Preview);
        EditorUtility.SetDirty(rune);
    }

    public void ResortRunes()
    {
        DuplicateKeyComparer<float> comparer = new();

        storage.runesHeight = new();

        foreach (Rune rune in storage.runes)
        {
            int hash = rune.GetHashCode();

            storage.runesHeight.Add(rune.averageHeight, hash);
        }

        File.WriteAllText("Assets/Resources/Runes/sorted.json", storage.runesHeight.ToString());

        storage.areSortedListsUpdated = true;
    }
}