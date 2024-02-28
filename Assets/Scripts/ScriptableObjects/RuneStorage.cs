using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "RuneStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    [property: SerializeField] public HashSet<Rune> Runes { get; private set; } = new();


    private void OnEnable()
    {
        Runes.AddRange(Resources.LoadAll<Rune>("Runes"));
    }


    public void NewRune()
    {
        Rune rune = CreateInstance<Rune>();
        rune.previewPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Textures/Runes/Previews/preview.asset");
        AssetDatabase.CreateAsset(new Texture2D(128, 128), rune.previewPath);
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

        rune.avaregeMass = (rune.avaregeMass * rune.drawVariations.Count + variation.points.Length) / (rune.drawVariations.Count + 1);
        rune.avaregeMassCenter = (rune.avaregeMassCenter * rune.drawVariations.Count + variation.massCenter) / (rune.drawVariations.Count + 1);
        rune.averageHeight = (rune.averageHeight * rune.drawVariations.Count + variation.height) / (rune.drawVariations.Count + 1);

        rune.drawVariations.Add(variation);

        foreach (Vector2 point in variation.points)
        {
            rune.Preview.SetPixel((int)(point.x * rune.Preview.width), (int)(point.y * rune.Preview.height), Color.black);
        }

        rune.Preview.Apply();
        EditorUtility.SetDirty(rune.Preview);

        EditorUtility.SetDirty(rune);
    }
}