using AYellowpaper.SerializedCollections;
using SquallOfSpells.RuneSystem;
using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    [CreateAssetMenu(fileName = "Spell storage", menuName = "Scriptable objects/Spell storage")]
    public class SpellStorage : ScriptableObject
    {
        [SerializeField] public SerializedDictionary<Rune, GameObject> spells = new();
    }
}