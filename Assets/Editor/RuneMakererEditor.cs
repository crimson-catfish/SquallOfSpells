using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RuneMaker))]
public class RuneMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RuneMaker runeMaker = (RuneMaker)target;

        GUIContent[] imagesToChooseFrom = new GUIContent[runeMaker.runeStorage.runes.Count];
        for (int i = 0; i < runeMaker.runeStorage.runes.Count; i++)
        {
            imagesToChooseFrom[i] = new GUIContent(runeMaker.runeStorage.runes[i].preview);
        }

        runeMaker.runeToEditIndex = EditorGUILayout.Popup
        (
            label:new GUIContent("rune to edit"),
            selectedIndex:runeMaker.runeToEditIndex,
            displayedOptions:imagesToChooseFrom
        );
        
        if (GUILayout.Button("New rune")) runeMaker.NewRune();
    }
}