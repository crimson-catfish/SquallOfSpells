using AYellowpaper.SerializedCollections;
using SquallOfSpells.SigilSystem;
using UnityEngine;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Spell storage", menuName = "Scriptable objects/Spell storage")]
    public class ActionStorage : ScriptableObject
    {
        [SerializeField] public SerializedDictionary<Sigil, GameObject> spells           = new();
        [SerializeField] public SerializedDictionary<Sigil, GameObject> pauseMenuActions = new();
        [SerializeField] public SerializedDictionary<Sigil, GameObject> mainMenuActions  = new();
    }
}