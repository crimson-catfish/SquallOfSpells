using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RuneMaker : MonoBehaviour
{
    [SerializeField] private InputManager         inputManager;
    [SerializeField] private RuneStorage          storage;
    [SerializeField] private RuneDrawManager      drawManager;
    [SerializeField] private RuneTogglesContainer togglesContainer;
    [SerializeField] private ToggleGroup          toggleGroup;
    [SerializeField] private RuneLimbo            limbo;

    [SerializeField] private int minimalPointAmount = 8;

    [Header("Rune preview related fields\nchanging those doesn't affects already created previews")]
    [SerializeField] private TextureFormat textureFormat;
    [SerializeField]              private int   width         = 128;
    [SerializeField]              private int   border        = 8;
    [SerializeField]              private int   pointRadius   = 4;
    [SerializeField, Range(0, 1)] private float pointDarkness = 0.3f;

    private void OnEnable()
    {
        inputManager.OnAddVariation += AddCurrentVariationToCurrentRune;
        inputManager.OnNewRune += SaveCurrentVariationToNewRune;
        inputManager.OnDeleteRune += DeleteCurrentRune;
    }

    private void OnDisable()
    {
        while (limbo.runesToDelete.Count > 0)
        {
            Rune rune = limbo.runesToDelete[0];
            AssetDatabase.DeleteAsset(rune.previewPath);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rune));
            limbo.runesToDelete.RemoveAt(0);
        }

        inputManager.OnAddVariation -= AddCurrentVariationToCurrentRune;
        inputManager.OnNewRune -= SaveCurrentVariationToNewRune;
        inputManager.OnDeleteRune -= DeleteCurrentRune;
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

        togglesContainer.AddNewToggle(rune);

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

        Undo.RegisterFullObjectHierarchyUndo(togglesContainer, "remove toggle from container");
        Undo.RecordObject(toggleGroup.GetFirstActiveToggle().gameObject, "disable toggle object");
        togglesContainer.RemoveToggle(rune);

        Undo.RecordObject(limbo, "add rune to limbo");
        limbo.runesToDelete.Add(rune);
        
        Undo.SetCurrentGroupName("delete rune");
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