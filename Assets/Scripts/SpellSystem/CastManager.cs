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

        private void OnEnable()
        {
            recognizer.OnRecognized += HandleRecognized;
        }

        private void OnDisable()
        {
            recognizer.OnRecognized -= HandleRecognized;
        }


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

            spell.Cast();
        }
    }
}