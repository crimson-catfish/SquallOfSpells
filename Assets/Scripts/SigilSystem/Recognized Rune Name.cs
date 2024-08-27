using TMPro;
using UnityEngine;

namespace SquallOfSpells.SigilSystem
{
    public class RecognizedRuneName : MonoBehaviour
    {
        [SerializeField] private SigilRecognizer  recognizer;
        [SerializeField] private TextMeshProUGUI textMeshPro;


        private void OnEnable()
        {
            recognizer.OnRecognized += rune =>
            {
                // Debug.Assert(rune != null, "rune != null");
                // image.texture = rune == null ? null : rune.Preview;

                textMeshPro.text = rune == null ? "" : rune.name;
            };
        }
    }
}