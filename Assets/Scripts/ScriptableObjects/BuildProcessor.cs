#if UNITY_EDITOR

using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

[CreateAssetMenu(fileName = "MyCustomBuildProcessor", menuName = "ScriptableObjects/MyCustomBuildProcessor")]
class CustomBuildProcessor : ScriptableObject, IPreprocessBuildWithReport
{
    [SerializeField] private RuneStorage storage;

    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        if (!storage.areSortedListsUpdated) RuneMaker.instance.ResortRunes();
    }
}

#endif