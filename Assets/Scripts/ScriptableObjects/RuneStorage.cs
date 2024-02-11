using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RuneStorage", menuName = "ScriptableObjects/RuneStorage")]
public class RuneStorage : ScriptableObject
{
    [Header("DO NOT MODIFY")]
    [SerializeField] public List<Rune> runes = new();
}