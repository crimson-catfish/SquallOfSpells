using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ToggleGroup), typeof(VerticalLayoutGroup))]
public class RuneMaker : MonoBehaviour
{
    [SerializeField] private RuneStorage storage;
    [SerializeField] private RuneDrawManager drawManager;
    [SerializeField] private RuneRecognizer recognizer;
    [SerializeField] private GameObject runePrefab;

    [SerializeField] private int minimalPointAmount = 8;

    [Header("Rune preview related fields\nchanging those doesn't affects already created previews")] [SerializeField]
    private int width = 128;

    [SerializeField] private int border = 8;
    [SerializeField] private int pointRadius = 4;
    [SerializeField, Range(0, 1)] private float pointDarkness = 0.3f;
    [SerializeField] private TextureFormat textureFormat;

    private Vector2 scrollPosition;
    private ToggleGroup toggleGroup;
    private readonly Dictionary<int, Toggle> toggles = new();
    private Rune currentRecognized;


    private void OnEnable()
    {
        recognizer.OnRuneRecognized += rune => currentRecognized = rune;
    }


    private void Start()
    {
        toggleGroup = this.GetComponent<ToggleGroup>();
        foreach (Rune rune in storage.Runes.Values)
            AddRuneToggleToScrollView(rune);
    }

    private void OnDisable()
    {
        recognizer.OnRuneRecognized -= rune => currentRecognized = rune;
    }


    public void SaveCurrentVariationToNewRune()
    {
        RuneDrawVariation variation = drawManager.currentVariation;
        if (!DoesVariationHaveEnoughPoints(variation)) return;

        Rune rune = ScriptableObject.CreateInstance<Rune>();
        AssetDatabase.CreateAsset(rune, AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Runes/rune.asset"));

        rune.Preview = new Texture2D(width, width, textureFormat, false);
        rune.previewPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Textures/Runes/Previews/preview.asset");
        AssetDatabase.CreateAsset(rune.Preview, rune.previewPath);
        AddCurrentVariationToRune(rune);

        Toggle toggle = AddRuneToggleToScrollView(rune);
        toggle.isOn = true;

        storage.AddRune(rune);
    }

    public void AddCurrentVariationToCurrentRune()
    {
        Toggle activeToggle = toggleGroup.GetFirstActiveToggle();
        if (activeToggle == null)
        {
            Debug.LogWarning("Select rune to add to.");
            return;
        }

        Rune rune = activeToggle.gameObject.GetComponent<RuneToggle>().Rune;

        Undo.IncrementCurrentGroup();

        Undo.RecordObject(rune, "add draw variation");
        Undo.RecordObject(rune.Preview, "update preview");
        AddCurrentVariationToRune(rune);

        Undo.SetCurrentGroupName("add draw variation to " + rune.name);
    }

    public void DeleteCurrentRune()
    {
        Toggle activeToggle = toggleGroup.GetFirstActiveToggle();
        if (activeToggle == null)
        {
            Debug.LogWarning("No rune selected. Nothing to delete.");
            return;
        }

        Rune rune = activeToggle.gameObject.GetComponent<RuneToggle>().Rune;

        Undo.IncrementCurrentGroup();

        Undo.RecordObject(storage, "delete rune from storage");
        storage.DeleteRune(rune);

        Undo.RegisterFullObjectHierarchyUndo(this, "remove toggle");
        toggles.Remove(rune.GetHashCode());
        toggleGroup.SetAllTogglesOff();

        Undo.DestroyObjectImmediate(activeToggle.gameObject);

        Undo.DestroyObjectImmediate(rune.Preview);

        Undo.DestroyObjectImmediate(rune);

        Undo.SetCurrentGroupName("delete rune");
    }

    public void SelectRecognizedRune()
    {
        if (currentRecognized == null)
            return;

        if (toggles.TryGetValue(currentRecognized.GetHashCode(), out Toggle toggle))
            toggle.isOn = true;
    }

    private Toggle AddRuneToggleToScrollView(Rune rune)
    {
        GameObject runeToggleObject = Instantiate(runePrefab, this.transform);

        if (runeToggleObject.TryGetComponent(out Toggle toggle))
            toggle.group = toggleGroup;

        if (runeToggleObject.TryGetComponent(out RuneToggle runeToggle))
            runeToggle.Rune = rune;

        if (runeToggleObject.TryGetComponent(out AspectRatioFitter ratioFitter))
            ratioFitter.aspectRatio = (float)rune.Preview.width / (float)rune.Preview.height;

        toggles.Add(rune.GetHashCode(), toggle);


        return runeToggleObject.GetComponent<Toggle>();
    }

    private bool DoesVariationHaveEnoughPoints(RuneDrawVariation variation)
    {
        if (variation == null)
        {
            Debug.LogWarning("Draw something to save");
            return false;
        }

        if (variation.points.Length <= minimalPointAmount)
        {
            Debug.LogWarning("Too few points in rune, didn't save");
            return false;
        }

        return true;
    }

    private void AddCurrentVariationToRune(Rune rune)
    {
        RuneDrawVariation variation = drawManager.currentVariation;
        if (!DoesVariationHaveEnoughPoints(variation)) return;
        if (rune.drawVariations.Contains(variation)) return;

        // update rune data
        rune.averageMassCenter = (rune.averageMassCenter * rune.drawVariations.Count + variation.massCenter) /
                                 (rune.drawVariations.Count + 1);
        rune.averageHeight = (rune.averageHeight * rune.drawVariations.Count + variation.height) /
                             (rune.drawVariations.Count + 1);
        if (!rune.drawVariations.Contains(variation))
            rune.drawVariations.Add(variation);

        // resize preview texture
        Texture2D tex = rune.Preview;
        Texture2D oldPreview = new(tex.width, tex.height, textureFormat, false);
        Graphics.CopyTexture(tex, oldPreview);
        tex.Reinitialize(width, (int)math.max(tex.height, variation.height * width));
        tex.SetPixels(0, (tex.height - oldPreview.height) / 2, oldPreview.width, oldPreview.height,
            oldPreview.GetPixels(0, 0, oldPreview.width, oldPreview.height));
        if (toggles.TryGetValue(rune.GetHashCode(), out Toggle toggle))
            toggle.GetComponent<RuneToggle>().ratioFitter.aspectRatio = tex.height / tex.width;


        // add new draw variation on preview texture
        foreach (Vector2 point in variation.points)
        {
            int x = (int)(point.x * (tex.width - border * 2)) + border - pointRadius;
            int y = (int)(point.y / variation.height * (tex.height - border * 2)) + border - pointRadius;

            Color[] colors = tex.GetPixels(x, y, pointRadius, pointRadius);
            for (int i = 1; i < colors.Length; ++i)
                colors[i] *= 1 - pointDarkness;
            tex.SetPixels(x, y, pointRadius, pointRadius, colors);
        }

        // save
        rune.Preview.Apply();
        EditorUtility.SetDirty(rune.Preview);
        EditorUtility.SetDirty(rune);
    }
}