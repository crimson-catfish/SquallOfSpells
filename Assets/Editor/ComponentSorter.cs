using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class ComponentsSorter : ScriptableObject
{
    static ComponentsSorter()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    [MenuItem("Edit/Sort Components %&a")]
    private static void SortComponentsMenuItem()
    {
        SortComponentsInAllGameObjects();
    }

    private static void OnHierarchyChanged()
    {
        if (Selection.activeGameObject == null)
            return;

        SortComponents(Selection.activeGameObject);
    }

    private static void SortComponentsInAllGameObjects()
    {
        // Sort components in all loaded scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            foreach (GameObject gameObject in scene.GetRootGameObjects())
            {
                SortComponentsInHierarchy(gameObject);
            }
        }

        // Sort components in all prefabs
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string prefabGuid in prefabGuids)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            SortComponentsInHierarchy(prefab);
        }
    }

    private static void SortComponentsInHierarchy(GameObject gameObject)
    {
        SortComponents(gameObject);

        foreach (Transform child in gameObject.transform)
        {
            SortComponentsInHierarchy(child.gameObject);
        }
    }

    private static void SortComponents(GameObject gameObject)
    {
        List<Component> sortedComponents = gameObject.GetComponents<Component>()
            .Where(component => !(component is Transform))
            .OrderBy(component => IsCustomComponent(component) ? 0 : 1)
            .ToList();

        for (int i = 0; i < sortedComponents.Count; i++)
        {
            MoveComponentToIndex(gameObject, sortedComponents[i], i);
        }
    }

    private static void MoveComponentToIndex(GameObject gameObject, Component component, int index)
    {
        List<Component> components = gameObject.GetComponents<Component>()
            .Where(comp => !(comp is Transform))
            .ToList();

        int currentIndex = components.IndexOf(component);

        while (currentIndex > index)
        {
            ComponentUtility.MoveComponentUp(component);
            currentIndex--;
        }

        while (currentIndex < index)
        {
            ComponentUtility.MoveComponentDown(component);
            currentIndex++;
        }
    }

    private static bool IsCustomComponent(Component component)
    {
        string ns = component.GetType().Namespace;

        if (ns == null)
            return true;

        return !ns.StartsWith("UnityEngine");
    }
}