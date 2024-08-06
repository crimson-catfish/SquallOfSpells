using AYellowpaper.SerializedCollections;
using SquallOfSpells.RuneSystem;
using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public class SpellContainer : MonoBehaviour
    {
        [SerializeField] private RuneRecognizer                         recognizer;
        [SerializeField] private SerializedDictionary<Rune, GameObject> spells = new();
        [SerializeField] private InputManager                           inputManager;


        private void OnEnable()
        {
            recognizer.OnRuneRecognized += HandleRuneRecognized;
        }

        private void HandleRuneRecognized(Rune rune)
        {
            if (rune == null)
                return;

            if (!spells.TryGetValue(rune, out var spellObject))
                return;

            if (spellObject.TryGetComponent(out IAimable aimableSpell))
            {
                inputManager.SwitchToActionMap(inputManager.Controls.Aim);
                inputManager.OnAimCast += aimableSpell.Cast;
            }
        }
    }
}