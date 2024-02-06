using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RunesStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    [SerializeField] public List<Rune> runes = new();
}