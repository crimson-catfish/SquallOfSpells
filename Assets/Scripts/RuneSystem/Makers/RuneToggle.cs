using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle), typeof(RectTransform), typeof(AspectRatioFitter))]
public class RuneToggle : MonoBehaviour
{
    [HideInInspector]
    public Rune Rune
    {
        get => rune;
        set
        {
            rune = value;
            rawImage.texture = rune.Preview;
        }
    }

    public AspectRatioFitter ratioFitter;

    [SerializeField] private RawImage rawImage;
    [SerializeField] private Toggle toggle;
    [SerializeField] private Outline outline;


    [SerializeField] private float scrollSpeed = 5f;


    [Header("Transition color settings")] [SerializeField]
    private Color normalColor = new(1f, 1f, 1f, 0.75f);

    [SerializeField] private Color selectedColor = new(0.74f, 0.74f, 0.74f);
    [SerializeField] private Color outlineColorSelected = new(0f, 0f, 0f, 0.82f);

    private Rune rune;
    private ScrollRect scrollRect;
    private RectTransform rectTransform;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
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
            StartCoroutine(scrollRect.FocusOnItemCoroutine(rectTransform, scrollSpeed));
        }
        else
        {
            rawImage.color = normalColor;
            outline.effectColor = Color.clear;
        }
    }
}