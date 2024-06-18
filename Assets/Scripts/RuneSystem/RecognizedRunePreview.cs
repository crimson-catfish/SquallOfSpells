using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class RecognizedRunePreview : MonoBehaviour
{
    [SerializeField] private RuneRecognizer recognizer;


    private RawImage image;

    private void OnEnable()
    {
        recognizer.OnRuneRecognized += HandleRuneRecognition;
    }

    private void Start()
    {
        image = GetComponent<RawImage>();
    }

    private void OnDisable()
    {
        recognizer.OnRuneRecognized -= HandleRuneRecognition;
    }


    private void HandleRuneRecognition(Rune rune)
    {
        image.texture = rune == null ? null : rune.Preview;
    }
}