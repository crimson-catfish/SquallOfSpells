using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OldRuneImporter))]
public class OldRuneImporterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        OldRuneImporter runeImporter = (OldRuneImporter)target;

        if (GUILayout.Button("Import new")) runeImporter.ImportNew();
        if (GUILayout.Button("Reimport all")) runeImporter.ReimportAll();
    }
}