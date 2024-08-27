#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

namespace SquallOfSpells.SigilSystem.Creating
{
    [RequireComponent(typeof(RawImage))]
    public class RecognizedSigilPreview : MonoBehaviour
    {
        [SerializeField] private SigilRecognizer recognizer;

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


        private void HandleRecognition(Sigil sigil)
        {
            if (sigil is null)
            {
                image.texture = null;

                return;
            }

            rectTransform.sizeDelta = new Vector2
            (
                rectTransform.rect.height * sigil.Preview.width / sigil.Preview.height,
                rectTransform.rect.height
            );

            image.texture = sigil.Preview;
        }
    }
}
#endif