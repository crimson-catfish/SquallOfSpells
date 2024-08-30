using AYellowpaper.SerializedCollections;
using SquallOfSpells.SigilSystem;
using UnityEngine;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Action storage", menuName = "Scriptable objects/Action storage")]
    public class ActionStorage : ScriptableObject
    {
        [SerializeField] public SerializedDictionary<Sigil, GameObject> pauseMenuActions = new();
        [SerializeField] public SerializedDictionary<Sigil, GameObject> mainMenuActions  = new();
    }
}