using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

[Serializable]
class CustomBuildProcessor : IPreprocessBuildWithReport
{
    [SerializeField] public RuneStorage storage;

    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        if (!storage.areSortedListsUpdated) RuneMaker.instance.ResortRunes();
    }
}