using TMPro;
using UnityEngine;

namespace SquallOfSpells.SigilSystem
{
    public class RecognizedSigilName : MonoBehaviour
    {
        [SerializeField] private SigilRecognizer recognizer;
        [SerializeField] private TextMeshProUGUI textMeshPro;


        private void OnEnable()
        {
            recognizer.OnRecognized += sigil =>
            {
                // Debug.Assert(rune != null, "rune != null");
                // image.texture = rune == null ? null : rune.Preview;

                textMeshPro.text = sigil == null ? "" : sigil.name;
            };
        }
    }
}