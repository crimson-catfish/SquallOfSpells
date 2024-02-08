using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RunesStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    [Header("DO NOT MODIFY")]
    [SerializeField] public List<Rune> runes = new();
}