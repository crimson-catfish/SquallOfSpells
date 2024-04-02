using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    public Dictionary<int, Rune> runes = new();

    public SortedList<float, int> runesHeight;
    public SortedList<float, int> runesMassCenterX;
    public SortedList<float, int> runesMassCenterY;

    public bool areSortedListsUpdated = false;


    private void OnEnable()
    {
        runes.Clear();
        foreach (Rune rune in Resources.LoadAll<Rune>("Runes")) runes.Add(rune.GetHashCode(), rune);
        runesHeight = JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/height").text);
        runesMassCenterX = JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/massCenterX").text);
        runesMassCenterY = JsonConvert.DeserializeObject<SortedList<float, int>>(Resources.Load<TextAsset>("Runes/massCenterX").text);
    }
}