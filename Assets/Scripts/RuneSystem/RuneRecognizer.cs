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
        HashSet<int> hashSet = new HashSet<int>(FindClosestRunesByParams(runeDraw.height, storage.runesHeight, heightRange));
        hashSet.IntersectWith(FindClosestRunesByParams(runeDraw.massCenter.x, storage.runesMassCenterX, massCenterRange));
        hashSet.IntersectWith(FindClosestRunesByParams(runeDraw.massCenter.y, storage.runesMassCenterY, massCenterRange));
        hashSet.IntersectWith(FindClosestRunesByParams(runeDraw.points.Length, storage.runesMass, massRange));
    }

    private IEnumerable<int> FindClosestRunesByParams(float runeParam, SortedList<float, int> sortedRunes, float range)
    {
        int lowBound = Search.Binary<float>(sortedRunes.Keys, runeParam - range);
        int topBound = Search.Binary<float>(sortedRunes.Keys, runeParam + range);

        return sortedRunes.Values.Skip(lowBound).Take(topBound - lowBound);
    }
}
