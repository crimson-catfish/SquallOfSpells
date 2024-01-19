using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RuneImporter))]
public class RuneImporterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RuneImporter runeImporter = (RuneImporter)target;

        if (GUILayout.Button("Import new")) runeImporter.ImportNew();
        if (GUILayout.Button("Reimport all")) runeImporter.ReimportAll();
    }
}