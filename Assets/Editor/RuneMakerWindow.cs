using UnityEngine;
using UnityEditor;
using System.Linq;

public class RuneMakerWindow : EditorWindow
{
    [SerializeField] private RuneStorage storage;
    
    private RuneDrawVariation drawVariation = null;
    private RuneDrawManager drawManager;
    private Vector2 scrollPosition;
    private Texture2D emptyTexture;


    private void Awake()
    {
        drawManager = RuneDrawManager.instance;
        Debug.Log(drawManager);

        emptyTexture = new Texture2D(128, 128, TextureFormat.ARGB32, false);
    }

    private void OnEnable()
    {
        drawManager.OnNewDrawVariation += CasheNewRuneDrawVariation;
    }

    private void OnDisable()
    {
        drawManager.OnNewDrawVariation -= CasheNewRuneDrawVariation;
    }


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
        Debug.Log(drawVariation);
        if (drawVariation == null)
        {
            Debug.Log("Draw something to save");
            return;
        }
        storage.runes[whereToSave].AddNewRuneDrawVariation(drawVariation);
    }

    public void DeleteCurrentRune(int runeToDeleteIndex)
    {
        storage.runes[runeToDeleteIndex].FreePreviewTextureFromAssets();
        storage.runes.RemoveAt(runeToDeleteIndex);
    }

    public void NewRune()
    {
        storage.runes.Add(new Rune(emptyTexture));
    }

    private void CasheNewRuneDrawVariation(RuneDrawVariation variation)
    {
        Debug.Log("cashed");
        drawVariation = variation;
    }
}