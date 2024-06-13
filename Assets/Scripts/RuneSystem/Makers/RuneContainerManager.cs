using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ToggleGroup), typeof(VerticalLayoutGroup))]
public class RuneContainerManager : MonoBehaviour
{
    [SerializeField] private GameObject runePrefab;
    [SerializeField] private RuneStorage storage;
    [SerializeField] private RuneMaker runeMaker;
    [SerializeField] private RuneDrawManager drawManager;

    private Vector2 scrollPosition;
    private TMP_Dropdown dropdown;
    private ToggleGroup toggleGroup;
    private GameObject currentRuneGUI;

    private void Start()
    {
        toggleGroup = this.GetComponent<ToggleGroup>();
        foreach (Rune rune in storage.runes.Values)
            AddRune(rune);
    }

    private void OnSelect(bool isSelected)
    {
        if (!isSelected) return;

        Toggle toggle = toggleGroup.GetFirstActiveToggle();
        
        currentRuneGUI = toggle.gameObject;
        
        if (toggle.gameObject.TryGetComponent(out RuneGUI runeGUI))
            runeMaker.currentRune = runeGUI.rune;
    }

    public void AddRune(Rune rune)
    {
        GameObject runeGUI = Instantiate(runePrefab, this.transform);
        
        if (runeGUI.TryGetComponent(out RawImage image))
            image.texture = rune.Preview;
        
        if (runeGUI.TryGetComponent(out RuneGUI runeGUIScript))
            runeGUIScript.rune = rune;
        
        if (runeGUI.TryGetComponent(out Toggle toggle))
        {
            toggle.group = toggleGroup;
            toggle.onValueChanged.AddListener(OnSelect);
        }
    }

    public void UpdateSelected(Rune rune)
    {
        if (currentRuneGUI.TryGetComponent(out RawImage image))
            image.texture = rune.Preview;
        
        if (currentRuneGUI.TryGetComponent(out RuneGUI runeGUI))
            runeGUI.rune = rune;
    }

    public void DeleteSelected()
    {
        if (currentRuneGUI == null)
        {
            Debug.LogWarning("No rune selected. Nothing to delete.");
            return;
        }
        
        Destroy(currentRuneGUI.gameObject);
    }
}