#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

namespace SquallOfSpells.SigilSystem.RuneCreating
{
    [RequireComponent(typeof(RawImage))]
    public class RecognizedRunePreview : MonoBehaviour
    {
        [SerializeField] private RuneRecognizer recognizer;

        private RectTransform rectTransform;
        private RawImage      image;


        private void Start()
        {
            rectTransform = this.GetComponent<RectTransform>();
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
            if (rune is null)
            {
                image.texture = null;

                return;
            }

            rectTransform.sizeDelta = new Vector2
            (
                rectTransform.rect.height * rune.Preview.width / rune.Preview.height,
                rectTransform.rect.height
            );

            image.texture = rune.Preview;
        }
    }
}
#endif