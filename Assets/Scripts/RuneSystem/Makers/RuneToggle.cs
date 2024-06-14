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

    private Rune rune;
}