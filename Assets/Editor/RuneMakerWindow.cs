using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class RuneMakerWindow : EditorWindow
{
    [SerializeField] private RuneStorage storage;

    private Vector2 scrollPosition;
    private RuneMaker runeMaker;
    private Rune lastRune = null;


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

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

        int runeIndex = 0;

        foreach (Rune rune in storage.runes.Values)
        {
            EditorGUILayout.BeginHorizontal();


            GUILayout.Box(rune.Preview);


            EditorGUILayout.BeginVertical();


            if (GUILayout.Button("Add draw variation"))
            {
                runeMaker.AddDrawVariation(rune);
                lastRune = rune;
            }
            if (GUILayout.Button("Delete current rune"))
            {
                GUILayout.Label("Base Settings", EditorStyles.boldLabel);
                if (EditorUtility.DisplayDialog("Deleting warning", "Delete this rune (can't undo this action)?", "OK", "cancel"))
                {
                    runeMaker.DeleteRune(rune);
                    lastRune = null;
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

        if (GUILayout.Button("Save draw variation to new rune")) lastRune = runeMaker.SaveDrawVariationToNewRune();

        if (GUILayout.Button("Resort saved runes (may take some time)")) ResortRunes();

        EditorGUILayout.EndHorizontal();

        // keyboard shortcuts
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            switch (e.keyCode)
            {
                case KeyCode.A:
                    runeMaker.AddDrawVariation(lastRune);
                    break;
                case KeyCode.N:
                    lastRune = runeMaker.SaveDrawVariationToNewRune();
                    break;
                case KeyCode.Delete:
                    if (EditorUtility.DisplayDialog("Deleting warning", "Delete last edited rune (can't undo this action)?", "OK", "cancel"))
                    {
                        runeMaker.DeleteRune(lastRune);
                        lastRune = null;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void ResortRunes()
    {
        EditorUtility.DisplayProgressBar("Resorting runes", "In process", 0);
        runeMaker.ResortRunes();
        EditorUtility.ClearProgressBar();
    }
}