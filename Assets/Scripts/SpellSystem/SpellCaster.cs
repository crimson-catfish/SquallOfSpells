using SquallOfSpells.RuneSystem;
using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public class SpellCaster : MonoBehaviour
    {
        [SerializeField] private SpellStorage   storage;
        [SerializeField] private RuneRecognizer recognizer;
        [SerializeField] private InputManager   inputManager;

        private void HandleRuneRecognized(Rune rune)
        {
            if (rune == null)
                return;

            if (!storage.spells.TryGetValue(rune, out var spellObject))
                return;

            if (spellObject.TryGetComponent(out IAimable aimableSpell))
            {
                inputManager.SwitchToActionMap(inputManager.Controls.Aim);
                inputManager.OnAimCast += aimableSpell.Cast;
                inputManager.OnAimCast += _ => inputManager.OnAimCast -= aimableSpell.Cast;
            }
        }

        private void OnEnable()
        {
            recognizer.OnRuneRecognized += HandleRuneRecognized;
        }
    }
}