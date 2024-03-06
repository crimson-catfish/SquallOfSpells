using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RuneRecognizer : MonoBehaviour
{
    [SerializeField] private RuneStorage storage;

    [Header("Recognition settings")]
    [SerializeField] private float heightRange;
    [SerializeField] private float massCenterRange;
    [SerializeField] private float massRange;


    private RuneDrawManager drawManager;

    private void Awake()
    {
        drawManager = RuneDrawManager.instance;
    }

    private void OnEnable()
    {
        drawManager.RuneDrawn += (runeDraw) => OnRuneDrawn(runeDraw);
    }

    private void OnDisable()
    {
        drawManager.RuneDrawn -= (runeDraw) => OnRuneDrawn(runeDraw);
    }

    private void OnRuneDrawn(RuneDrawVariation runeDraw)
    {
        HashSet<int> runesToCheck = new HashSet<int>(FindClosestRunesByParams(runeDraw.height, storage.runesHeight, heightRange));
        foreach (var v in runesToCheck) Debug.Log(v);
        Debug.Log("================");
        runesToCheck.IntersectWith(FindClosestRunesByParams(runeDraw.massCenter.x, storage.runesMassCenterX, massCenterRange));
        foreach (var v in runesToCheck) Debug.Log(v);
        Debug.Log("================");
        runesToCheck.IntersectWith(FindClosestRunesByParams(runeDraw.massCenter.y, storage.runesMassCenterY, massCenterRange));
        foreach (var v in runesToCheck) Debug.Log(v);
        Debug.Log("================");
        runesToCheck.IntersectWith(FindClosestRunesByParams(runeDraw.points.Length, storage.runesMass, massRange));
        foreach (var v in runesToCheck) Debug.Log(v);
        Debug.Log("================");
    }

    private IEnumerable<int> FindClosestRunesByParams(float runeParam, SortedList<float, int> sortedRunes, float range)
    {
        int lowBound = Search.Binary<float>(sortedRunes.Keys, runeParam - range);
        int topBound = Search.Binary<float>(sortedRunes.Keys, runeParam + range);

        return sortedRunes.Values.Skip(lowBound).Take(topBound - lowBound);
    }
}
