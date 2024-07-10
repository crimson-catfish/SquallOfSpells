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

        if (!spells.TryGetValue(rune, out var spellObject))
            return;

        if (spellObject.TryGetComponent(out IAimable aimableSpell))
        {
            inputManager.SwitchToActionMap(inputManager.Controls.Aim);
            inputManager.OnAimCast += aimableSpell.Cast;
        }
        else if (spellObject.TryGetComponent(out ISwingable swingableSpell))
        {
            inputManager.SwitchToActionMap(inputManager.Controls.Swing);
            // inputManager.On
        }
    }
}