using System.Collections.Generic;
using UnityEngine;

public static class Closest
{
    public static PointAndDistance GetPointAndDistance(Vector2 pointToCheck, IEnumerable<Vector2> pointsField)
    {
        PointAndDistance closest = new() { sqrDistance = Mathf.Infinity };
        foreach (Vector2 point in pointsField)
        {
            float sqrDistance = (point - pointToCheck).sqrMagnitude;
            if (sqrDistance < closest.sqrDistance)
            {
                closest.sqrDistance = sqrDistance;
                closest.point = point;
            }
        }
        return closest;
    }

    public static float GetSqrDistance(Vector2 pointToCheck, IEnumerable<Vector2> pointsField)
    {
        float minSqrDistance = Mathf.Infinity;
        foreach (Vector2 point in pointsField)
        {
            float sqrDistance = (point - pointToCheck).sqrMagnitude;
            if (sqrDistance < minSqrDistance) minSqrDistance = sqrDistance;
        }
        return minSqrDistance;
    }

    public struct PointAndDistance
    {
        public Vector2 point;
        public float sqrDistance;
    }
}
