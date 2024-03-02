#if UNITY_EDITOR

using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

class MyCustomBuildProcessor : IPreprocessBuildWithReport
{
    [SerializeField] private RuneStorage storage;

    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        if (!storage.IsSorted) storage.SortRunes();
    }
}

#endif