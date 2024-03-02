using UnityEngine;

public class RuneRecognizer : MonoBehaviour
{
    [SerializeField] private RuneStorage storage;

    private RuneDrawManager drawManager;

    private void Awake()
    {
        drawManager = RuneDrawManager.instance;
    }

    private void OnEnable()
    {
        drawManager.RuneDrawn += (runeDraw) => OnRuneDrawn(runeDraw);
    }

    private void OnDisable()
    {
        drawManager.RuneDrawn -= (runeDraw) => OnRuneDrawn(runeDraw);
    }

    private void OnRuneDrawn(RuneDrawVariation runeDraw)
    {
        foreach (Rune runeToCheck in storage.Runes)
        {
            
        }
    }
}
