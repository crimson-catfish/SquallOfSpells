using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using SquallOfSpells.SigilSystem;
using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public class CastManager : MonoBehaviour
    {
        [SerializeField] private SigilRecognizer recognizer;
        [SerializeField] private InputManager    inputManager;

        [SerializeField] private SerializedDictionary<Sigil, GameObject> spells;


        private void HandleRecognized(Sigil sigil)
        {
            if (sigil is null)
                return;

            if (!spells.ContainsKey(sigil))
            {
                Debug.LogWarning("No corresponding spell for " + sigil.name);

                return;
            }

            GameObject spellObject = spells[sigil];
            ICastable spell = spellObject.GetComponent<ICastable>();


            switch (spell)
            {
                case IInstant instant:
                    instant.Cast();

                    break;

                case IDirectable directable:
                    if (spellObject.TryGetComponent(out SpriteRenderer spriteRenderer))
                    {
                        spriteRenderer.enabled = true;

                        inputManager.OnAimCast += _ => spriteRenderer.enabled = false;
                    }

                    inputManager.SwitchToActionMap(inputManager.Controls.Aim);

                    inputManager.OnAimCast += directable.Cast;
                    inputManager.OnAimCast += _ => inputManager.OnAimCast -= directable.Cast;

                    break;
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