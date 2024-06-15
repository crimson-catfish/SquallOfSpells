using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
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

    [SerializeField] private RawImage rawImage;
    [SerializeField] private Toggle toggle;
    [SerializeField] private Outline outline;


    [Header("Transition color settings")] [SerializeField]
    private Color normalColor = new(1f, 1f, 1f, 0.75f);

    [SerializeField] private Color selectedColor = new(0.74f, 0.74f, 0.74f);
    [SerializeField] private Color outlineColorSelected = new Color(0f, 0f, 0f, 0.82f);


    private Rune rune;

    private void OnEnable()
    {
        rawImage.color = normalColor;
        outline.effectColor = Color.clear;
        toggle.onValueChanged.AddListener(HandleValueChange);
    }

    private void HandleValueChange(bool value)
    {
        if (value)
        {
            rawImage.color = selectedColor;
            outline.effectColor = outlineColorSelected;
        }
        else
        {
            rawImage.color = normalColor;
            outline.effectColor = Color.clear;
        }
    }
}