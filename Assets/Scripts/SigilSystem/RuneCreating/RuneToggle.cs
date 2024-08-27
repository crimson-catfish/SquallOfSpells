#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

namespace SquallOfSpells.SigilSystem.RuneCreating
{
    [RequireComponent(typeof(Toggle), typeof(RectTransform), typeof(AspectRatioFitter))]
    public class RuneToggle : MonoBehaviour
    {
        [SerializeField] private AspectRatioFitter ratioFitter;
        [SerializeField] private RawImage          rawImage;
        [SerializeField] private Toggle            toggle;
        [SerializeField] private Outline           outline;

        [SerializeField] private float scrollSpeed = 5f;

        [Header("Transition color settings")]
        [SerializeField] private Color normalColor = new(1f, 1f, 1f, 0.75f);
        [SerializeField] private Color selectedColor        = new(0.74f, 0.74f, 0.74f);
        [SerializeField] private Color outlineColorSelected = new(0f, 0f, 0f, 0.82f);

        private Rune       rune;
        private ScrollRect scrollRect;

        public Rune Rune {
            get => rune;
            set
            {
                rune = value;
                rawImage.texture = rune.Preview;
                ratioFitter.aspectRatio = rawImage.texture.height / (float)rawImage.texture.width;
            }
        }

        private void OnEnable()
        {
            rawImage.color = normalColor;
            outline.effectColor = Color.clear;
            ratioFitter = GetComponent<AspectRatioFitter>();

            toggle.onValueChanged.AddListener(HandleValueChange);

            if (this.transform.parent.parent.parent.TryGetComponent(out ScrollRect scroll))
                scrollRect = scroll;
        }

        private void HandleValueChange(bool value)
        {
            if (value)
            {
                rawImage.color = selectedColor;
                outline.effectColor = outlineColorSelected;
                StartCoroutine(scrollRect.FocusOnItemCoroutine(GetComponent<RectTransform>(), scrollSpeed));
            }
            else
            {
                rawImage.color = normalColor;
                outline.effectColor = Color.clear;
            }
        }
    }
}
#endif