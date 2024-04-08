using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class RuneMaker : MonoBehaviour
{
    [SerializeField] private RuneStorage storage;
    [SerializeField] private GameObject dropdownContainer;
    [SerializeField] private RuneDrawManager drawManager;

    [Header("changing this properties doesn't affects already created previews\nPlease recreate them to apply changes")]
    [SerializeField] private int size;
    [SerializeField] private int border;
    [SerializeField] private int pointRadius;
    [SerializeField] private Color pointColor;

    private Vector2 scrollPosition;
    private Rune lastRune = null;
    private Color[] pointColors;
    private Dropdown dropdown;
    private bool areRunesSorted = false;

    private void OnEnable()
    {
        pointColors = new Color[pointRadius * pointRadius];
        for (int i = 0; i < pointRadius * pointRadius; i++) pointColors[i] = pointColor;
    }

    private void Start()
    {
        dropdown = dropdownContainer.GetComponent<Dropdown>();
        foreach (Rune rune in storage.runes.Values)
        {
            if (rune == null) print("no rune");
            if (dropdown == null) print("no dropdown");
            dropdown.options.Add(new Dropdown.OptionData(rune.Preview));
        }
    }

    private void OnDisable()
    {
        ResortRunes();
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
                AddDrawVariation(rune);
                lastRune = rune;
            }
            if (GUILayout.Button("Delete current rune"))
            {
                GUILayout.Label("Base Settings", EditorStyles.boldLabel);
                if (EditorUtility.DisplayDialog("Deleting warning", "Delete this rune (can't undo this action)?", "OK", "cancel"))
                {
                    DeleteRune(rune);
                    lastRune = null;
                    OnGUI();
                    return;
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();


        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save draw variation to new rune")) lastRune = SaveDrawVariationToNewRune();

        if (GUILayout.Button("Resort saved runes (may take some time)")) ResortRunes();

        EditorGUILayout.EndHorizontal();

        // keyboard shortcuts
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            switch (e.keyCode)
            {
                case KeyCode.A:
                    AddDrawVariation(lastRune);
                    break;
                case KeyCode.N:
                    lastRune = SaveDrawVariationToNewRune();
                    break;
                case KeyCode.Delete:
                    if (EditorUtility.DisplayDialog("Deleting warning", "Delete last edited rune (can't undo this action)?", "OK", "cancel"))
                    {
                        DeleteRune(lastRune);
                        lastRune = null;
                    }
                    break;
            }
        }
    }

    private Rune SaveDrawVariationToNewRune()
    {
        Rune rune = ScriptableObject.CreateInstance<Rune>();
        rune.previewPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Sprites/Runes/Previews/preview.asset");
        AssetDatabase.CreateAsset(rune, AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Runes/rune.asset"));
        AssetDatabase.CreateAsset
        (
            Sprite.Create
            (
                new Texture2D(size, size, TextureFormat.ARGB32, false),
                new Rect(0.0f, 0.0f, size, size),
                new Vector2(0.5f, 0.5f)
            ),
            rune.previewPath
        );
        storage.runes.Add(rune.GetHashCode(), rune);

        AddDrawVariation(rune);
        areRunesSorted = false;
        return rune;
    }

    private void DeleteRune(Rune rune)
    {
        storage.runes.Remove(rune.GetHashCode());
        AssetDatabase.DeleteAsset(rune.previewPath);
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rune));
        areRunesSorted = false;
    }

    private void AddDrawVariation(Rune rune)
    {
        RuneDrawVariation variation = drawManager.drawVariation;

        if (variation == null)
        {
            Debug.LogWarning("Draw something to save");
            return;
        }
        if (variation.points.Length <= 5)
        {
            Debug.LogWarning("Too few points in rune, didn't save");
            return;
        }
        else print(variation.points.Length);
        if (rune.drawVariations.Contains(variation)) return;

        // update rune data
        rune.averageMassCenter = (rune.averageMassCenter * rune.drawVariations.Count + variation.massCenter) /
                                 (rune.drawVariations.Count + 1);
        rune.averageHeight = (rune.averageHeight * rune.drawVariations.Count + variation.height) /
                             (rune.drawVariations.Count + 1);
        if (!rune.drawVariations.Contains(variation)) rune.drawVariations.Add(variation);

        // resize preview texture
        var texture = rune.Preview.texture;
        Texture2D oldPreview = new(texture.width, texture.height, TextureFormat.ARGB32, false);
        Graphics.CopyTexture(rune.Preview.texture, oldPreview);
        texture.Reinitialize(size, (int)math.max(texture.height, variation.height * size));
        texture.SetPixels(0, (texture.height - oldPreview.height) / 2, oldPreview.width, oldPreview.height,
            oldPreview.GetPixels(0, 0, oldPreview.width, oldPreview.height));


        // add new draw variation on preview texture
        foreach (Vector2 point in variation.points)
        {
            int x = (int)(point.x * (texture.width - border * 2)) + border - pointRadius;
            int y = (int)(point.y / variation.height * (texture.height - border * 2)) + border - pointRadius;

            Color[] colors = texture.GetPixels(x, y, pointRadius, pointRadius);
            for (int i = 1; i < colors.Length; ++i) colors[i] += pointColor;
            texture.SetPixels(x, y, pointRadius, pointRadius, colors);
        }

        // save
        rune.Preview.texture.Apply();
        EditorUtility.SetDirty(rune.Preview);
        EditorUtility.SetDirty(rune);
        
        areRunesSorted = false;
    }

    private void ResortRunes()
    {
        if (areRunesSorted) return;
        
        EditorUtility.DisplayProgressBar("Resorting runes", "In process", 0);

        storage.runesHeight.Clear();
        storage.runesMassCenterX.Clear();
        storage.runesMassCenterY.Clear();
        
        foreach (KeyValuePair<int, Rune> rune in storage.runes)
        {
            storage.runesHeight.Add(rune.Value.averageHeight, rune.Key);
            storage.runesMassCenterX.Add(rune.Value.averageMassCenter.x, rune.Key);
            storage.runesMassCenterY.Add(rune.Value.averageMassCenter.y, rune.Key);
        }

        File.WriteAllText("Assets/Resources/Runes/height.json", JsonConvert.SerializeObject(storage.runesHeight));
        File.WriteAllText("Assets/Resources/Runes/massCenterX.json", JsonConvert.SerializeObject(storage.runesMassCenterX));
        File.WriteAllText("Assets/Resources/Runes/massCenterY.json", JsonConvert.SerializeObject(storage.runesMassCenterY));

        areRunesSorted = true;
        
        EditorUtility.ClearProgressBar();
    }
}