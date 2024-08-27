using System;
using SquallOfSpells.SigilSystem;
using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public class SpellCaster : MonoBehaviour
    {
        [SerializeField] private SpellStorage   storage;
        [SerializeField] private SigilRecognizer recognizer;
        [SerializeField] private InputManager   inputManager;


        private void HandleRecognized(Sigil sigil)
        {
            if (sigil == null)
                return;

            if (!storage.spells.TryGetValue(sigil, out GameObject spellObject))
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
            recognizer.OnRecognized += HandleRecognized;
        }

        private void OnDisable()
        {
            recognizer.OnRecognized -= HandleRecognized;
        }
    }
}