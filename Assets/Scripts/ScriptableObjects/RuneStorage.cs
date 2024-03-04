using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RuneStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    public HashSet<Rune> runes= new();

    public SortedList<float, int> runesHeight;
    
    public bool areSortedListsUpdated = false;


    private void OnEnable()
    {
        foreach (Rune rune in Resources.LoadAll<Rune>("Runes")) runes.Add(rune);
    }
}