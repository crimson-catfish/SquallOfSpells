using System.Collections.Generic;
using System.Linq;
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
        //Debug.Log(FindClosestRunesByParams(runeDraw.height, storage.runesHeight, heightRange));
        // FindClosestRunesByParams(runeDraw.massCenter.x, storage.RunesMassCenterX, massCenterRange);
        // FindClosestRunesByParams(runeDraw.massCenter.y, storage.RunesMassCenterY, massCenterRange);
        // Debug.Log(FindClosestRunesByParams(runeDraw.points.Length, storage.RunesMass, massRange));
        Debug.Log("recognized");
    }

    private IEnumerable<Rune> FindClosestRunesByParams(float runeParam, SortedList<float, int> sortedRunes, float range)
    {
        int lowBound = Search.Binary<float>(sortedRunes.Keys, runeParam - range);
        int topBound = Search.Binary<float>(sortedRunes.Keys, runeParam + range);
        
        return storage.runes.Where(rune => sortedRunes.Keys.Skip(lowBound).Take(topBound - lowBound).Contains(rune.GetHashCode()));
    }
}
