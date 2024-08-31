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
        [FormerlySerializedAs("clickCasters"), FormerlySerializedAs("aimableSpells"), SerializeField]
        private ClickSpells clickSpells;

        [SerializeField] private SerializedDictionary<Sigil, GameObject> spells;


        private void HandleRecognized(Sigil sigil)
        {
            if (sigil == null)
                return;

            if (!spells.TryGetValue(sigil, out GameObject spellObject))
                return;


            if (spellObject.TryGetComponent(out IClick clickCaster))
            {
                aimableSpell.AimInit();

                inputManager.SwitchToActionMap(inputManager.Controls.Aim);

                inputManager.OnAimCast += clickCaster.Cast; 
                inputManager.OnAimCast += _ => inputManager.OnAimCast -= clickCaster.Cast;
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