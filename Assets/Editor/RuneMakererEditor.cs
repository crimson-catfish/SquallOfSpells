using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RuneMaker))]
public class RuneMakerEdtor : Editor
{
    [SerializeField] private RuneStorage runeStorage;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RuneMaker runeMaker = (RuneMaker)target;


        if (GUILayout.Button("Save draw variation")) runeMaker.SaveRuneDrawVariation();
        if (GUILayout.Button("New rune")) runeMaker.NewRune();
        if (GUILayout.Button("Delete current rune")) runeMaker.DeleteCurrentRune();

        foreach (Rune rune in runeStorage.runes)
        {
            GUILayout.Button(rune.Preview);
        }
    }
}