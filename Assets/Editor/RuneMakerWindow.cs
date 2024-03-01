using UnityEngine;
using UnityEditor;

public class RuneMakerWindow : EditorWindow
{
    [SerializeField] private RuneStorage storage;
    
    private Vector2 scrollPosition;


    [MenuItem("RuneSystem/Rune Maker")]
    public static void ShowWindow()
    {
        RuneMakerWindow window = (RuneMakerWindow)GetWindow(typeof(RuneMakerWindow), true, "Rune Maker");
        window.Show();
    }

    void OnGUI(){
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true); 

        int runeIndex = 0;

        foreach (Rune rune in storage.Runes)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Box(rune.Preview);

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Save draw variation")) storage.AddDrawVariation(rune);
            if (GUILayout.Button("Delete current rune"))
            {
                GUILayout.Label("Base Settings", EditorStyles.boldLabel);
                if (EditorUtility.DisplayDialog("Deleting warning", "Delete this rune?", "OK", "cancel"))
                {
                    storage.DeleteRune(rune);
                    OnGUI();
                    return;
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
            
            runeIndex++;
        }

        GUILayout.EndScrollView();  

        if (GUILayout.Button("New rune")) storage.NewRune();;
    }
}