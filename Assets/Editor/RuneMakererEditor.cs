using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RuneMaker))]
public class RuneMakerEdtor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RuneMaker runeMaker = (RuneMaker)target;
        
        if (GUILayout.Button("Save draw variation")) runeMaker.SaveRuneDrawVariation();
        if (GUILayout.Button("New rune")) runeMaker.NewRune();
        if (GUILayout.Button("Delete current rune")) runeMaker.DeleteCurrentRune();

        Texture2D texture = Resources.Load<Texture2D>("Textures/Runes/defaultRunePreview");
        
        GUILayout.Button(texture);
    }
}