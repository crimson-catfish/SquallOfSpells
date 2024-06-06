using TMPro;
using UnityEditor;
using UnityEngine;

public class RuneMakerGUI : MonoBehaviour
{
    [SerializeField] private GameObject dropdownContainer;
    [SerializeField] private RuneStorage storage;
    [SerializeField] private RuneMaker runeMaker;
    [SerializeField] private RuneDrawManager drawManager;

    private Vector2 scrollPosition;
    private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown = dropdownContainer.GetComponent<TMP_Dropdown>();
        foreach (Rune rune in storage.runes.Values)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData
                (
                    Sprite.Create
                    (
                        rune.Preview,
                        new Rect(0.0f, 0.0f, rune.Preview.width, rune.Preview.height),
                        new Vector2(0.5f, 0.5f)
                    )
                )
            );
        }
    }

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

        foreach (Rune rune in storage.runes.Values)
        {
            EditorGUILayout.BeginHorizontal();


            EditorGUILayout.BeginVertical();


            if (GUILayout.Button("Add draw variation"))
            {
                RuneDrawVariation variation = drawManager.drawVariation;
                runeMaker.AddDrawVariation(rune, variation);
            }

            if (GUILayout.Button("Delete current rune"))
            {
                GUILayout.Label("Base Settings", EditorStyles.boldLabel);
                if (EditorUtility.DisplayDialog(
                        "Deleting warning",
                        "Delete this rune (can't undo this action)?",
                        "OK",
                        "cancel"))
                {
                    runeMaker.DeleteRune(rune);
                    return;
                }
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();


        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save draw variation to new rune"))
        {
            RuneDrawVariation variation = drawManager.drawVariation;
            runeMaker.SaveDrawVariationToNewRune(variation);
        }

        if (GUILayout.Button("Resort saved runes (may take some time)")) runeMaker.ResortRunes();

        EditorGUILayout.EndHorizontal();
    }
}