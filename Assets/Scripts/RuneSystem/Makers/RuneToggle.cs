using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage), typeof(Toggle))]
public class RuneToggle : MonoBehaviour
{
    [HideInInspector] public Rune Rune
    {
        get => rune;
        set
        {
            rune = value;
            rawImage.texture = rune.Preview;
        }
    }

    private Rune rune;
    private RawImage rawImage;

    private void OnEnable()
    {
        rawImage = GetComponent<RawImage>();
    }
}