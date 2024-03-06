using UnityEngine;
using UnityEditor;

public class RuneMakerWindow : EditorWindow
{
    [SerializeField] private RuneStorage storage;

    private Vector2 scrollPosition;
    private RuneMaker runeMaker;


    private void OnEnable()
    {
        runeMaker = RuneMaker.instance;
    }


    [MenuItem("RuneSystem/Rune Maker")]
    public static void ShowWindow()
    {
        RuneMakerWindow window = (RuneMakerWindow)GetWindow(typeof(RuneMakerWindow), true, "Rune Maker");
        window.Show();
    }

    void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

        int runeIndex = 0;

        foreach (Rune rune in storage.runes.Values)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Box(rune.Preview);

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Add draw variation")) runeMaker.AddDrawVariation(rune);
            if (GUILayout.Button("Delete current rune"))
            {
                GUILayout.Label("Base Settings", EditorStyles.boldLabel);
                if (EditorUtility.DisplayDialog("Deleting warning", "Delete this rune (can't undo this action)?", "OK", "cancel"))
                {
                    runeMaker.DeleteRune(rune);
                    OnGUI();
                    return;
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            runeIndex++;
        }

        GUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save draw variation to new rune")) runeMaker.SaveDrawVariationToNewRune();
        if (GUILayout.Button("Resort saved runes (may take some time)")) ResortRunes();
        EditorGUILayout.EndHorizontal();
    }

    private void ResortRunes()
    {
        EditorUtility.DisplayProgressBar("Resorting runes", "In process", 0);
        runeMaker.ResortRunes();
        EditorUtility.ClearProgressBar();
    }
}