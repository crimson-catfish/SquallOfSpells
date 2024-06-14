using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ToggleGroup), typeof(VerticalLayoutGroup))]
public class RuneMaker : MonoBehaviour
{
    [SerializeField] private RuneStorage storage;
    [SerializeField] private RuneDrawManager drawManager;
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

    private void Start()
    {
        toggleGroup = this.GetComponent<ToggleGroup>();
        foreach (Rune rune in storage.runes.Values)
            AddRuneToggleToScrollView(rune);

        toggleGroup.SetAllTogglesOff();
    }


    public void SaveCurrentVariationToNewRune()
    {
        RuneDrawVariation variation = drawManager.currentVariation;
        if (!DoesVariationHaveEnoughPoints(variation)) return;

        Rune rune = ScriptableObject.CreateInstance<Rune>();
        rune.previewPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Textures/Runes/Previews/preview.asset");
        AssetDatabase.CreateAsset(rune, AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Runes/rune.asset"));
        AssetDatabase.CreateAsset(new Texture2D(width, width, TextureFormat.ARGB32, false), rune.previewPath);
        storage.runes.Add(rune.GetHashCode(), rune);

        AddCurrentVariationToRune(rune);
        AddRuneToggleToScrollView(rune);
    }

    public void AddCurrentVariationToCurrentRune()
    {
        Toggle activeToggle = toggleGroup.GetFirstActiveToggle();
        if (activeToggle == null)
        {
            Debug.LogWarning("Select rune to add to.");
            return;
        }
        
        AddCurrentVariationToRune(activeToggle.gameObject.GetComponent<RuneToggle>().Rune);
    }

    public void DeleteCurrentRune()
    {
        if (toggleGroup.GetFirstActiveToggle() == null)
        {
            Debug.LogWarning("No rune selected. Nothing to delete.");
            return;
        }

        storage.DeleteRune(toggleGroup.GetFirstActiveToggle().gameObject.GetComponent<RuneToggle>().Rune);
        Destroy(toggleGroup.GetFirstActiveToggle().gameObject);

        toggleGroup.SetAllTogglesOff();
    }

    private void AddRuneToggleToScrollView(Rune rune)
    {
        GameObject runeToggleObject = Instantiate(runePrefab, this.transform);

        if (runeToggleObject.TryGetComponent(out Toggle toggle))
            toggle.group = toggleGroup;

        if (runeToggleObject.TryGetComponent(out RuneToggle runeToggle))
            runeToggle.Rune = rune;
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
        if (!rune.drawVariations.Contains(variation)) rune.drawVariations.Add(variation);

        // resize preview texture
        Texture2D tex = rune.Preview;
        Texture2D oldPreview = new(tex.width, tex.height, TextureFormat.ARGB32, false);
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