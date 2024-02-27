using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RuneStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    [property: SerializeField] public List<Rune> Runes { get; private set; } = new();

    private int savedRunesCount = 0;
 

    private void OnEnable()
    {
        for (int i = 0; i < savedRunesCount; i++)
        {
            Runes.Add(BinarySerializer.Load<Rune>("rune"+i));
        }
    }

    private void OnDisable()
    {
        Runes.Clear();
    }

    public void AddRune(Rune rune)
    {
        savedRunesCount++;
        Runes.Add(rune);
    }

    public void DeleteRune(int runeToDeleteIndex)
    {
        Runes.RemoveAt(runeToDeleteIndex);
        savedRunesCount--;
    }
}