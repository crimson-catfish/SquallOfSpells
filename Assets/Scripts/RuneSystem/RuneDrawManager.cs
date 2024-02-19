using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneDrawManager : Singleton<RuneDrawManager>
{
    public RuneDrawVariation drawVariation;

    [SerializeField] private RuneDrawParameters parameters;
    [SerializeField] private GameObject pointPrefab;
    
    private InputManager inputManager;
    private Vector2 momentSum = Vector2.zero;
    private Rect drawFrame = new();
    private List<Vector2> drawPoints = new();
    private Vector2 lastPoint;


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
            momentSum = Vector2.zero;
            drawFrame = new Rect(nextDrawPosition.x, nextDrawPosition.y, 0, 0);
            CreateNewPoint(nextDrawPosition);
            return;
        }

        // check for the last point firstly becouse it's likely to be too close and then we don't need to do all heavy calculations
        if ((nextDrawPosition - lastPoint).magnitude < parameters.requairedDistance) return;

        while ((lastPoint - nextDrawPosition).magnitude >= parameters.requairedDistance)
        {
            Vector2 pointToCheck = lastPoint + ((nextDrawPosition - lastPoint).normalized * parameters.requairedDistance);

            Closest closest = FindClosestPoint(pointToCheck);


            // check if distance is long enough
            if (closest.sqrDistance >= (parameters.requairedDistance * parameters.requairedDistance * (1 - parameters.acceptableError)))
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
        drawPoints.Clear();
    }

    private void HeavyCheck(Vector2 nextDrawPosition, Vector2 pointToCheck)
    {
        Closest closest = FindClosestPoint(pointToCheck);

        for 
        (   
            int currentStep = parameters.heavyCheckStep;
            currentStep <= (nextDrawPosition - pointToCheck).magnitude;
            currentStep += parameters.heavyCheckStep
        ) {
            pointToCheck += (pointToCheck - lastPoint).normalized * currentStep;
            
            closest = FindClosestPoint(pointToCheck);

            if (closest.sqrDistance >= (parameters.requairedDistance * parameters.requairedDistance * 0.99))
            {
                CreateNewPoint(pointToCheck);
                return;
            }
        }
        lastPoint = closest.point;
    }

    private Closest FindClosestPoint(Vector2 pointToCheck) 
    {
        Closest closest = new() { sqrDistance = Mathf.Infinity };
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

    private void CreateNewPoint(Vector2 position)
    {
        Instantiate(pointPrefab, new Vector3(position.x, position.y, 0f), Quaternion.identity);
        lastPoint = position;
        drawPoints.Add(position);
        momentSum += position;
        if (position.x > drawFrame.xMax) drawFrame.xMax = position.x;
        if (position.x < drawFrame.xMin) drawFrame.xMin = position.x;
        if (position.y > drawFrame.yMax) drawFrame.yMax = position.y;
        if (position.y < drawFrame.yMin) drawFrame.yMin = position.y;
    }
    
    private void PrepareRuneVariation()
    {
        drawVariation = new()
        {
            points = new Vector2[drawPoints.Count],
            height = drawFrame.height / drawFrame.width
        };

        Vector2 ratioFactor = new(1, drawVariation.height);
        drawVariation.massCenter = Rect.PointToNormalized(drawFrame, momentSum / drawPoints.Count) * ratioFactor;
        for (int i = 0; i < drawPoints.Count; i++)
        {
            drawVariation.points[i] = Rect.PointToNormalized(drawFrame, drawPoints[i]) * ratioFactor;
        }
    }
}
