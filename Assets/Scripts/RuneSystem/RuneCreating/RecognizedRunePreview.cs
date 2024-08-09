#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

namespace SquallOfSpells.RuneSystem.RuneCreating
{
    [RequireComponent(typeof(RawImage))]
    public class RecognizedRunePreview : MonoBehaviour
    {
        [SerializeField] private RuneRecognizer recognizer;

        private RawImage image;


        private void Start()
        {
            image = GetComponent<RawImage>();
        }

        private void OnEnable()
        {
            recognizer.OnRecognized += HandleRecognition;
        }

        private void OnDisable()
        {
            recognizer.OnRecognized -= HandleRecognition;
        }


        private void HandleRecognition(Rune rune)
        {
            image.texture = rune == null ? null : rune.Preview;
        }
    }
}
#endif