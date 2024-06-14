using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    public Dictionary<int, Rune> Runes { get; } = new();
    public SortedList<float, int> RunesHeight { get; private set; } = new(Comparer);
    public SortedList<float, int> RunesMassCenterX { get; private set; } = new(Comparer);
    public SortedList<float, int> RunesMassCenterY { get; private set; } = new(Comparer);

    private static readonly DuplicateKeyComparer<float> Comparer = new();
    private bool areRunesSorted;


    private void OnEnable()
    {
        Runes.Clear();
        foreach (Rune rune in Resources.LoadAll<Rune>("Runes"))
            Runes.Add(rune.GetHashCode(), rune);

        RunesHeight =
            JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/height").text);
        RunesMassCenterX =
            JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/massCenterX").text);
        RunesMassCenterY =
            JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/massCenterX").text);
    }

    private void Sort()
    {
        if (areRunesSorted) return;

        EditorUtility.DisplayProgressBar("Resorting runes", "In process", 0);

        RunesHeight.Clear();
        RunesMassCenterX.Clear();
        RunesMassCenterY.Clear();

        foreach (KeyValuePair<int, Rune> rune in Runes)
        {
            RunesHeight.Add(rune.Value.averageHeight, rune.Key);
            RunesMassCenterX.Add(rune.Value.averageMassCenter.x, rune.Key);
            RunesMassCenterY.Add(rune.Value.averageMassCenter.y, rune.Key);
        }

        File.WriteAllText("Assets/Resources/Runes/height.json", JsonConvert.SerializeObject(RunesHeight));
        File.WriteAllText("Assets/Resources/Runes/massCenterX.json",
            JsonConvert.SerializeObject(RunesMassCenterX));
        File.WriteAllText("Assets/Resources/Runes/massCenterY.json",
            JsonConvert.SerializeObject(RunesMassCenterY));

        areRunesSorted = true;

        EditorUtility.ClearProgressBar();
    }


    public void AddRune(Rune rune)
    {
        Runes.Add(rune.GetHashCode(), rune);

        AddRuneProperties(rune);

        Serialize();
    }

    public void AddVariationToRune(RuneDrawVariation variation, Rune rune)
    {
        RemoveRuneProperties(rune);

        rune.drawVariations.Add(variation);

        AddRuneProperties(rune);
    }

    public void DeleteRune(Rune rune)
    {
        AssetDatabase.DeleteAsset(rune.previewPath);
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rune));

        Runes.Remove(rune.GetHashCode());
        RemoveRuneProperties(rune);

        Destroy(rune);
        Serialize();
    }

    private void AddRuneProperties(Rune rune)
    {
        RunesHeight.Add(rune.averageHeight, rune.GetHashCode());
        RunesMassCenterX.Add(rune.averageMassCenter.x, rune.GetHashCode());
        RunesMassCenterY.Add(rune.averageMassCenter.y, rune.GetHashCode());
    }

    private void RemoveRuneProperties(Rune rune)
    {
            RunesHeight.RemoveAt(RunesHeight.IndexOfValue(rune.GetHashCode()));
        RunesMassCenterX.RemoveAt(RunesMassCenterX.IndexOfValue(rune.GetHashCode()));
        RunesMassCenterY.RemoveAt(RunesMassCenterY.IndexOfValue(rune.GetHashCode()));
    }

    private void Serialize()
    {
        File.WriteAllText("Assets/Resources/Runes/height.json", JsonConvert.SerializeObject(RunesHeight));
        File.WriteAllText("Assets/Resources/Runes/massCenterX.json",
            JsonConvert.SerializeObject(RunesMassCenterX));
        File.WriteAllText("Assets/Resources/Runes/massCenterY.json",
            JsonConvert.SerializeObject(RunesMassCenterY));
    }
}