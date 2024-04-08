using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    public readonly Dictionary<int, Rune> runes = new();

    public SortedList<float, int> runesHeight = new();
    public SortedList<float, int> runesMassCenterX = new();
    public SortedList<float, int> runesMassCenterY = new();

    
    private void OnEnable()
    {
        runes.Clear();
        foreach (Rune rune in Resources.LoadAll<Rune>("Runes")) runes.Add(rune.GetHashCode(), rune);
        runesHeight = JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/height").text);
        runesMassCenterX = JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/massCenterX").text);
        runesMassCenterY = JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/massCenterX").text);
    }
}