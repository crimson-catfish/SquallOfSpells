using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "RunesStorage", menuName = "ScriptableObjects")]
public class RunesStorage : ScriptableObject
{
    public SerializedDictionary<string, RuneImage> runeImages = new();

    public void SaveNewRuneImage(RuneImage runeImageToSave)
    {
        runeImages[runeImageToSave.name] = runeImageToSave;
    }
}