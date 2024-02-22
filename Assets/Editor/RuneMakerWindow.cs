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
        while (runeIndex < storage.runes.Count)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Box(storage.runes[runeIndex].Preview);

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Save draw variation")) SaveDrawVariation(runeIndex);
            if (GUILayout.Button("Delete current rune"))
            {
                GUILayout.Label("Base Settings", EditorStyles.boldLabel);
                if (EditorUtility.DisplayDialog("Deleting warning", "Delete this rune?", "OK", "cancel"))
                {
                    DeleteCurrentRune(runeIndex);
                    continue;
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
            
            runeIndex++;
        }

        GUILayout.EndScrollView();  

        if (GUILayout.Button("New rune")) NewRune();
    }


    public void SaveDrawVariation(int whereToSave)
    {
        if (RuneDrawManager.instance.drawVariation == null || RuneDrawManager.instance.drawVariation.points.Length == 0)
        {
            Debug.Log("Draw something to save");
            return;
        }
        storage.runes[whereToSave].AddNewRuneDrawVariation(RuneDrawManager.instance.drawVariation);
    }

    public void DeleteCurrentRune(int runeToDeleteIndex)
    {
        storage.runes[runeToDeleteIndex].FreePreviewTextureFromAssets();
        storage.runes.RemoveAt(runeToDeleteIndex);
    }

    public void NewRune()
    {
        storage.runes.Add(new Rune().NewRune()); // couse unity sucks
    }
}