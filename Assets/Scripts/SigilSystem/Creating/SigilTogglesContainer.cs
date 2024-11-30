#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SquallOfSpells.SigilSystem.Creating
{
    public class SigilTogglesContainer : MonoBehaviour
    {
        [SerializeField]                                           private InputManager    inputManager;
        [SerializeField]                                           private SigilRecognizer recognizer;
        [FormerlySerializedAs("runeTogglePrefab"), SerializeField] private GameObject      sigilTogglePrefab;
        [SerializeField]                                           private SigilStorage    storage;
        [SerializeField]                                           private ToggleGroup     toggleGroup;

        private readonly Dictionary<int, Toggle> toggles = new();
        private          Sigil                   currentRecognized;
        private          Vector2                 scrollPosition;


        private void Start()
        {
            toggleGroup = GetComponent<ToggleGroup>();

            foreach (Sigil sigil in storage.sigils)
                AddToggle(sigil);
        }

        private void OnEnable()
        {
            recognizer.OnRecognized += HandleRecognition;
            inputManager.OnSelectRecognized += SelectRecognized;
        }

        private void OnDisable()
        {
            recognizer.OnRecognized -= HandleRecognition;
            inputManager.OnSelectRecognized -= SelectRecognized;
        }

        private void HandleRecognition(Sigil sigil)
        {
            currentRecognized = sigil;
        }

        public void AddNewToggle(Sigil sigil)
        {
            AddToggle(sigil).isOn = true;
        }

        public void RemoveToggle(Sigil sigil)
        {
            toggles.Remove(sigil.GetHashCode());
            toggleGroup.GetFirstActiveToggle().gameObject.SetActive(false);
            toggleGroup.SetAllTogglesOff();
        }


        private void SelectRecognized()
        {
            if (currentRecognized == null)
                return;

            if (toggles.TryGetValue(currentRecognized.GetHashCode(), out Toggle toggle))
                toggle.isOn = true;
        }

        private Toggle AddToggle(Sigil sigil)
        {
            GameObject sigilToggleObject = Instantiate(sigilTogglePrefab, this.transform);

            if (sigilToggleObject.TryGetComponent(out Toggle toggle))
                toggle.group = toggleGroup;

            if (sigilToggleObject.TryGetComponent(out SigilToggle sigilToggle))
                sigilToggle.Sigil = sigil;

            if (sigilToggleObject.TryGetComponent(out AspectRatioFitter ratioFitter))
                ratioFitter.aspectRatio = sigil.Preview.width / (float)sigil.Preview.height;

            toggles.Add(sigil.GetHashCode(), toggle);

            return sigilToggleObject.GetComponent<Toggle>();
        }
    }
}
#endif