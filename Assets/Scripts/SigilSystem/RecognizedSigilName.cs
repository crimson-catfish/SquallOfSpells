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
                Debug.Log(sigil == null ? "not recognized" : sigil.name);

                textMeshPro.text = sigil == null ? "" : sigil.name;
            };
        }
    }
}