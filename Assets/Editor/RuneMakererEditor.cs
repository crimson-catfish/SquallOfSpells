using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RuneMaker))]
public class RuneMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RuneMaker runeMaker = (RuneMaker)target;
        
        if (GUILayout.Button("New rune")) runeMaker.NewRune();
    }
}