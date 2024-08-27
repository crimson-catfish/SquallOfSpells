using AYellowpaper.SerializedCollections;
using SquallOfSpells.SigilSystem;
using UnityEngine;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Spell storage", menuName = "Scriptable objects/Spell storage")]
    public class SpellStorage : ScriptableObject
    {
        [SerializeField] public SerializedDictionary<Sigil, GameObject> spells = new();
    }
}