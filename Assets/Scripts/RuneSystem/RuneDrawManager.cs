using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RuneDrawManager : Singleton<RuneDrawManager>
{
    public event Action<RuneDrawVariation> RuneDrawn;

    [HideInInspector] public RuneDrawVariation drawVariation;

    [SerializeField] private RuneDrawParameters param;
    [SerializeField] private bool showDrawPoints;
    
    private InputManager inputManager;
    private Vector2 momentSum = Vector2.zero;
    private Rect drawFrame = new();
    private List<Vector2> drawPoints = new();
    private Vector2 lastPoint;
    private LineRenderer lineRenderer;
    private bool wasDrawEndPerformed = true;


    private void Awake()
    {
        inputManager = InputManager.instance;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        inputManager.OnNextDrawPosition += HandleNextDrawPosition;
        inputManager.OnDrawEnd += HandleDrawEnd;
    }

    private void Start()
    {
        Gizmos.color = Color.green;
    }

    private void OnDrawGizmos()
    {
        if (showDrawPoints)
        {
            foreach (Vector2 point in drawPoints)
            {
                Gizmos.DrawSphere((Vector3)point, param.requairedDistance / 2);
            }
        }
    }

    private void OnDisable()
    {
        inputManager.OnNextDrawPosition -= HandleNextDrawPosition;
        inputManager.OnDrawEnd += HandleDrawEnd;
    }


    private void HandleNextDrawPosition(Vector2 nextDrawPosition)
    {
        if (wasDrawEndPerformed)
        {
            wasDrawEndPerformed = false;

            ClearDrawing();
            drawPoints.Clear();
            momentSum = Vector2.zero;
            drawFrame = new Rect(nextDrawPosition.x, nextDrawPosition.y, 0, 0);
            CreateNewPoint(nextDrawPosition);
            return;
        }

        // check for the last point firstly becouse it's likely to be too close and then we don't need to do all heavy calculations
        if ((nextDrawPosition - lastPoint).magnitude < param.requairedDistance) return;

        while ((lastPoint - nextDrawPosition).magnitude >= param.requairedDistance)
        {
            Vector2 pointToCheck = lastPoint + ((nextDrawPosition - lastPoint).normalized * param.requairedDistance);

            Closest closest = FindClosestPoint(pointToCheck);


            // check if distance is long enough
            if (closest.sqrDistance >= (param.requairedDistance * param.requairedDistance * (1 - param.acceptableError)))
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
        RuneDrawn?.Invoke(drawVariation);
        wasDrawEndPerformed = true;
    }

    private void HeavyCheck(Vector2 nextDrawPosition, Vector2 pointToCheck)
    {
        Closest closest = FindClosestPoint(pointToCheck);

        for 
        (   
            int currentStep = param.heavyCheckStep;
            currentStep <= (nextDrawPosition - pointToCheck).magnitude;
            currentStep += param.heavyCheckStep
        ) {
            pointToCheck += (pointToCheck - lastPoint).normalized * currentStep;
            
            closest = FindClosestPoint(pointToCheck);

            if (closest.sqrDistance >= (param.requairedDistance * param.requairedDistance * 0.99))
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
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, (Vector3)position);

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


    public void ClearDrawing()
    {
        lineRenderer.positionCount = 0;
    }
}
