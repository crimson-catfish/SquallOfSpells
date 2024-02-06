using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneDrawManager : Singleton<RuneDrawManager>
{
    public Action<RuneDrawVariation> OnNewDrawVariation;

    [SerializeField] private GameObject pointPrefab;
    
    private InputManager inputManager;
    private Vector2 momentSum = Vector2.zero;
    private float[] drawFrame = new float[] { Mathf.Infinity, Mathf.NegativeInfinity, Mathf.Infinity, Mathf.NegativeInfinity };
    private List<Vector2> drawPoints = new();
    private Vector2 lastPointPosition;
    private float requairedDistanceBetweenPoints = 20;


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
        Debug.DrawLine(nextDrawPosition, nextDrawPosition + new Vector2(1, 1), Color.red, 1000);

        if (drawPoints.Count > 0)
        {
            // check if distance between previous and current position is too small for the optimisation sake
            if ((lastPointPosition - nextDrawPosition).magnitude < requairedDistanceBetweenPoints)
            {
                return;
            }
            else
            {
            }
        }
        else
        {
            CreateNewPoint(nextDrawPosition);
            return;
        }

        while (true)
        {
            // find the closest point and distance to it
            float minimalDistance = Mathf.Infinity;
            Vector2 closestPoint = new();
            foreach (Vector2 pointPosition in drawPoints)
            {
                float distance = (pointPosition - nextDrawPosition).magnitude;
                if (distance < minimalDistance) 
                {
                    minimalDistance = distance;
                    closestPoint = pointPosition;
                }
            }

            // check if distance is long enough
            if (minimalDistance < requairedDistanceBetweenPoints)
            {
                break;
            }

            CreateNewPoint(closestPoint + (nextDrawPosition - closestPoint).normalized * requairedDistanceBetweenPoints * 1f);
        }
    }

    private void HandleDrawEnd()
    {
        PrepareRuneVariation();
        ClearVariablesForNewRuneVariation();
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
        lastPointPosition = position;
        drawPoints.Add(position);
        momentSum += position;
        if (position.x < drawFrame[0]) drawFrame[0] = position.x;
        if (position.y < drawFrame[1]) drawFrame[1] = position.y;
        if (position.x > drawFrame[2]) drawFrame[2] = position.x;
        if (position.y > drawFrame[3]) drawFrame[3] = position.y;
    }
}
