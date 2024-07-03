using UnityEngine;

public class SpellContainer : MonoBehaviour
{
    [SerializeField] private RuneRecognizer recognizer;
    [SerializeField] private AYellowpaper.SerializedCollections.SerializedDictionary<Rune, GameObject> spells = new();

    private InputManager inputManager;

    private void OnEnable()
    {
        recognizer.OnRuneRecognized += HandleRuneRecognized;
        inputManager = InputManager.instance;
    }

    private void HandleRuneRecognized(Rune rune)
    {
        if (rune == null)
            return;

        ICastable spell = spells[rune].GetComponent<ICastable>();

        // if (spell is IAimable)
                        // inputManager.c

            spell.Cast();
    }
}