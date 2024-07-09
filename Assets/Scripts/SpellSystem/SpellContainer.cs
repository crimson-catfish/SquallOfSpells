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

        GameObject spellObject;
        if (!spells.TryGetValue(rune, out spellObject))
            return;

        if (spellObject.TryGetComponent<IAimable>(out var aimableSpell))
        {
            inputManager.SwitchToActionMap(inputManager.Controls.Aim);
            inputManager.OnAimCast += aimableSpell.Cast;
        }
        else if (spellObject.TryGetComponent<ISwingable>(out var swingableSpell))
        {
            inputManager.SwitchToActionMap(inputManager.Controls.Swing);
            // inputManager.On
        }
    }
}