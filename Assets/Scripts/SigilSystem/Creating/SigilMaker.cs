#if UNITY_EDITOR
using SquallOfSpells.SigilSystem.Draw;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SquallOfSpells.SigilSystem.Creating
{
    public class SigilMaker : MonoBehaviour
    {
        [SerializeField] private InputManager          inputManager;
        [SerializeField] private SigilStorage          storage;
        [SerializeField] private DrawManager           drawManager;
        [SerializeField] private SigilTogglesContainer togglesContainer;
        [SerializeField] private ToggleGroup           toggleGroup;
        [SerializeField] private SigilLimbo            limbo;

        [Header("Rune preview related fields\nchanging those doesn't affects already created previews")]
        [SerializeField] private TextureFormat textureFormat;
        [SerializeField] private int   width       = 128;
        [SerializeField] private int   border      = 8;
        [SerializeField] private int   pointRadius = 4;
        [SerializeField] private Color previewPointColor;

        private Sigil currentSigil;

        private void OnEnable()
        {
            inputManager.OnNewSigil += NewSigil;
            inputManager.OnDeleteSigil += DeleteCurrent;

            drawManager.OnSigilDrawn += sigil => currentSigil = sigil;
        }

        private void OnDisable()
        {
            while (limbo.sigilsToDelete.Count > 0)
            {
                Sigil sigil = limbo.sigilsToDelete[0];
                AssetDatabase.DeleteAsset(sigil.previewPath);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(sigil));
                limbo.sigilsToDelete.RemoveAt(0);
            }

            inputManager.OnNewSigil -= NewSigil;
            inputManager.OnDeleteSigil -= DeleteCurrent;
        }


        public void NewSigil()
        {
            AssetDatabase.CreateAsset(currentSigil,
                AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Sigils/sigil.asset"));

            currentSigil.Preview = new Texture2D(width, (int)(width * currentSigil.height), textureFormat, false);


            // add new draw variation on preview texture
            foreach (Vector2 point in currentSigil.points)
            {
                int x = (int)(point.x * (currentSigil.Preview.width - border * 2)) + border - pointRadius;

                int y = (int)(point.y / currentSigil.height * (currentSigil.Preview.height - border * 2)) +
                        border -
                        pointRadius;

                Color[] colors = currentSigil.Preview.GetPixels(x, y, pointRadius, pointRadius);

                for (int i = 1; i < colors.Length; ++i)
                    colors[i] = previewPointColor;

                currentSigil.Preview.SetPixels(x, y, pointRadius, pointRadius, colors);
            }


            currentSigil.previewPath =
                AssetDatabase.GenerateUniqueAssetPath("Assets/Textures/Sigils/Previews/preview.asset");

            AssetDatabase.CreateAsset(currentSigil.Preview, currentSigil.previewPath);

            EditorUtility.SetDirty(currentSigil.Preview);
            EditorUtility.SetDirty(currentSigil);


            togglesContainer.AddNewToggle(currentSigil);

            storage.sigils.Add(currentSigil);
        }


        public void DeleteCurrent()
        {
            Toggle activeToggle = toggleGroup.GetFirstActiveToggle();

            if (activeToggle == null)
            {
                Debug.LogWarning("No sigil selected. Nothing to delete.");

                return;
            }

            Sigil sigil = activeToggle.gameObject.GetComponent<SigilToggle>().Sigil;

            Undo.IncrementCurrentGroup();

            Undo.RecordObject(storage, "delete sigil from storage");
            storage.sigils.Add(sigil);

            Undo.RegisterFullObjectHierarchyUndo(togglesContainer, "remove toggle from container");
            Undo.RecordObject(toggleGroup.GetFirstActiveToggle().gameObject, "disable toggle object");
            togglesContainer.RemoveToggle(sigil);

            Undo.RecordObject(limbo, "add sigil to limbo");
            limbo.sigilsToDelete.Add(sigil);

            Undo.SetCurrentGroupName("delete sigil");
        }
    }
}
#endif