using TMPro;
using UnityEngine;

namespace SquallOfSpells.RuneSystem
{
    public class RecognizedRuneName : MonoBehaviour
    {
        [SerializeField] private RuneRecognizer  recognizer;
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