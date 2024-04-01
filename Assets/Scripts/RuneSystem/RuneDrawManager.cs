using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UILineRenderer))]
public class RuneDrawManager : Singleton<RuneDrawManager>
{
    public event Action<RuneDrawVariation> RuneDrawn;

    [HideInInspector] public RuneDrawVariation drawVariation;

    [SerializeField] private RuneDrawParameters param;
    [SerializeField] private bool showDrawPoints;

    private InputManager inputManager;
    private Vector2 momentSum = Vector2.zero;
    private Rect drawFrame;
    private List<Vector2> drawPoints = new();
    private Vector2 lastPoint;
    private UILineRenderer uiLineRenderer;
    private bool wasDrawEndPerformed = true;


    private void Awake()
    {
        inputManager = InputManager.instance;
        uiLineRenderer = GetComponent<UILineRenderer>();
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
        if (!showDrawPoints) return;
        foreach (Vector2 point in drawPoints)
        {
            Gizmos.DrawSphere(point, param.distanceBetweenPoints / 2);
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
        if ((nextDrawPosition - lastPoint).magnitude < param.distanceBetweenPoints) return;

        while ((lastPoint - nextDrawPosition).magnitude >= param.distanceBetweenPoints)
        {
            Vector2 pointToCheck = lastPoint + ((nextDrawPosition - lastPoint).normalized * param.distanceBetweenPoints);

            Closest.PointAndDistance closest = Closest.GetPointAndDistance(pointToCheck, drawPoints);


            // check if distance is long enough
            if (closest.sqrDistance >= (param.distanceBetweenPoints * param.distanceBetweenPoints * (1 - param.acceptableError)))
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
        Closest.PointAndDistance closest = Closest.GetPointAndDistance(pointToCheck, drawPoints);

        for
        (
            float currentStep = param.heavyCheckStep;
            currentStep <= (nextDrawPosition - pointToCheck).magnitude;
            currentStep += param.heavyCheckStep
        )
        {
            pointToCheck += (pointToCheck - lastPoint).normalized * currentStep;

            closest = Closest.GetPointAndDistance(pointToCheck, drawPoints);

            if (closest.sqrDistance >= (param.distanceBetweenPoints * param.distanceBetweenPoints * 0.99))
            {
                CreateNewPoint(pointToCheck);
                return;
            }
        }
        lastPoint = closest.point;
    }


    private void CreateNewPoint(Vector2 position)
    {
        uiLineRenderer.points.Add(position * new Vector2(Screen.width, Screen.height));
        uiLineRenderer.SetAllDirty();

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
        uiLineRenderer.points.Clear();
    }
}
