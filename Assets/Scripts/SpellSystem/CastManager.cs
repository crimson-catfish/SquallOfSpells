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
            if (sigil == null)
                return;

            if (!spells.TryGetValue(sigil, out GameObject spellObject))
                return;


            if (spellObject.TryGetComponent(out IClick clickCaster))
            {
                if (spellObject.TryGetComponent(out SpriteRenderer spriteRenderer))
                {
                    spriteRenderer.enabled = true;

                    inputManager.OnAimCast += _ => spriteRenderer.enabled = false;
                }

                inputManager.SwitchToActionMap(inputManager.Controls.Aim);

                inputManager.OnAimCast += clickCaster.Cast;
                inputManager.OnAimCast += _ => inputManager.OnAimCast -= clickCaster.Cast;
            }

            if (spellObject.TryGetComponent(out IHold holdCaster))
            {
            }

            if (spellObject.TryGetComponent(out IPositionable positionableSpell))
            {
                inputManager.SwitchToActionMap(inputManager.Controls.Aim);
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