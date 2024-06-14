using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    public readonly Dictionary<int, Rune> runes = new();

    public SortedList<float, int> runesHeight = new();
    public SortedList<float, int> runesMassCenterX = new();
    public SortedList<float, int> runesMassCenterY = new();

    private bool areRunesSorted;

    
    private void OnEnable()
    {
        runes.Clear();
        
        foreach (Rune rune in Resources.LoadAll<Rune>("Runes"))
            runes.Add(rune.GetHashCode(), rune);
        
        runesHeight = JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/height").text);
        runesMassCenterX = JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/massCenterX").text);
        runesMassCenterY = JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/massCenterX").text);
    }

    private void Sort()
    {
        if (areRunesSorted) return;

        EditorUtility.DisplayProgressBar("Resorting runes", "In process", 0);

        runesHeight.Clear();
        runesMassCenterX.Clear();
        runesMassCenterY.Clear();

        foreach (KeyValuePair<int, Rune> rune in runes)
        {
            runesHeight.Add(rune.Value.averageHeight, rune.Key);
            runesMassCenterX.Add(rune.Value.averageMassCenter.x, rune.Key);
            runesMassCenterY.Add(rune.Value.averageMassCenter.y, rune.Key);
        }

        File.WriteAllText("Assets/Resources/Runes/height.json", JsonConvert.SerializeObject(runesHeight));
        File.WriteAllText("Assets/Resources/Runes/massCenterX.json",
            JsonConvert.SerializeObject(runesMassCenterX));
        File.WriteAllText("Assets/Resources/Runes/massCenterY.json",
            JsonConvert.SerializeObject(runesMassCenterY));

        areRunesSorted = true;

        EditorUtility.ClearProgressBar();
    }
    
    public void DeleteRune(Rune rune)
    {
        AssetDatabase.DeleteAsset(rune.previewPath);
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rune));
        runes.Remove(rune.GetHashCode());
        Destroy(rune);
    }
}