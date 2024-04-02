#if UNITY_EDITOR

using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

[CreateAssetMenu(fileName = "MyCustomBuildProcessor", menuName = "ScriptableObjects/MyCustomBuildProcessor")]
class CustomBuildProcessor : ScriptableObject, IPreprocessBuildWithReport
{
    [SerializeField] private RuneStorage storage;

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (!storage.areSortedListsUpdated) RuneMaker.instance.ResortRunes();
    }
}

#endif