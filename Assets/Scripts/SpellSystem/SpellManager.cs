using System;
using AYellowpaper.SerializedCollections;
using SquallOfSpells.SigilSystem;
using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public class SpellManager : MonoBehaviour
    {
        [SerializeField] private SigilRecognizer recognizer;
        [SerializeField] private InputManager    inputManager;
        [SerializeField] private AimableSpells   aimableSpells;


        [SerializeField] private SerializedDictionary<Sigil, GameObject> spells;


        private void HandleRecognized(Sigil sigil)
        {
            if (sigil == null)
                return;

            if (!spells.TryGetValue(sigil, out GameObject spellObject))
                return;

            if (spellObject.TryGetComponent(out IAimable aimableSpell))
            {
                aimableSpell.AimInit();

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