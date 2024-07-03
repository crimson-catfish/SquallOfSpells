using UnityEngine;

public class SpellContainer : MonoBehaviour
{
    [SerializeField] private RuneRecognizer recognizer;
    [SerializeField] private AYellowpaper.SerializedCollections.SerializedDictionary<Rune, GameObject> spells = new();

    private void OnEnable()
    {
        recognizer.OnRuneRecognized += HandleRuneRecognized;
    }

    private void HandleRuneRecognized(Rune rune)
    {
        if (rune == null)
            return;

        ICastable spell = spells[rune].GetComponent<ICastable>();

        spell.Cast();
    }
}