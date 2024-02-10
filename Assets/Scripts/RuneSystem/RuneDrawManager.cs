using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneDrawManager : Singleton<RuneDrawManager>
{
    public Action<RuneDrawVariation> OnNewDrawVariation;

    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private int step = 10;
    
    private InputManager inputManager;
    private Vector2 momentSum = Vector2.zero;
    private float[] drawFrame = new float[] { Mathf.Infinity, Mathf.NegativeInfinity, Mathf.Infinity, Mathf.NegativeInfinity };
    private List<Vector2> drawPoints = new();
    private Vector2 lastPoint;
    private float requairedDistance = 20;


    private void Awake()
    {
        inputManager = InputManager.instance;
    }

    private void OnEnable()
    {
        inputManager.OnNextDrawPosition += HandleNextDrawPosition;
        inputManager.OnDrawEnd += HandleDrawEnd;
    }

    private void OnDisable()
    {
        inputManager.OnNextDrawPosition -= HandleNextDrawPosition;
        inputManager.OnDrawEnd += HandleDrawEnd;
    }


    private void HandleNextDrawPosition(Vector2 nextDrawPosition)
    {
        if (drawPoints.Count == 0)
        {
            CreateNewPoint(nextDrawPosition);
            return;
        }

        // check for the last point firstly becouse it's likely to be too close and then we don't need to do all heavy calculations
        if ((nextDrawPosition - lastPoint).magnitude < requairedDistance) return;

        while ((lastPoint - nextDrawPosition).magnitude >= requairedDistance)
        {
            Vector2 pointToCheck = lastPoint + ((nextDrawPosition - lastPoint).normalized * requairedDistance);

            Closest closest = FindClosestPoint(pointToCheck);


            // check if distance is long enough
            if (closest.sqrDistance >= (requairedDistance * requairedDistance * 0.99))
            {
                CreateNewPoint(pointToCheck);
            }
            else
            {
                HeavyCheck(nextDrawPosition, pointToCheck);
            }
        }
    }

    private void HandleDrawEnd()
    {
        PrepareRuneVariation();
        ClearVariablesForNewRuneVariation();
    }

    private void HeavyCheck(Vector2 nextDrawPosition, Vector2 pointToCheck)
    {
        Closest closest = FindClosestPoint(pointToCheck);

        for (int currentStep = step; currentStep <= (nextDrawPosition - pointToCheck).magnitude; currentStep += step)
        {
            pointToCheck += (pointToCheck - lastPoint).normalized * currentStep;
            
            closest = FindClosestPoint(pointToCheck);

            if (closest.sqrDistance >= (requairedDistance * requairedDistance * 0.99))
            {
                CreateNewPoint(pointToCheck);
                return;
            }
        }
        lastPoint = closest.point;
    }

    private Closest FindClosestPoint(Vector2 pointToCheck) 
    {
        Closest closest = new();
        closest.sqrDistance = Mathf.Infinity;
        foreach (Vector2 point in drawPoints)
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

    private struct Closest
    {
        public Vector2 point;
        public float sqrDistance;
    }

    private void PrepareRuneVariation()
    {
        RuneDrawVariation drawVariation = new();

        Vector2 size = new(drawFrame[2] - drawFrame[0], drawFrame[3] - drawFrame[1]);

        drawVariation.points = new Vector2[drawPoints.Count];
        for (int i = 0; i < drawPoints.Count; i++)
        {
            drawVariation.points[i] = drawPoints[i] / size;
        }

        drawVariation.mass = drawPoints.Count;
        drawVariation.massCenter = momentSum / drawPoints.Count;
        drawVariation.ratio = size.y / size.x;

        OnNewDrawVariation?.Invoke(drawVariation);
    }

    private void ClearVariablesForNewRuneVariation()
    {
        drawPoints.Clear();
        drawFrame = new float[] { Mathf.Infinity, Mathf.NegativeInfinity, Mathf.Infinity, Mathf.NegativeInfinity };
        momentSum = Vector2.zero;
    }

    private void CreateNewPoint(Vector2 position)
    {
        Instantiate(pointPrefab, new Vector3(position.x, position.y, 0f), Quaternion.identity);
        lastPoint = position;
        drawPoints.Add(position);
        momentSum += position;
        if (position.x < drawFrame[0]) drawFrame[0] = position.x;
        if (position.y < drawFrame[1]) drawFrame[1] = position.y;
        if (position.x > drawFrame[2]) drawFrame[2] = position.x;
        if (position.y > drawFrame[3]) drawFrame[3] = position.y;
    }
}
