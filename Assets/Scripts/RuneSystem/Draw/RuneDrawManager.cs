using System;
using System.Collections.Generic;
using UnityEngine;

namespace SquallOfSpells.RuneSystem.Draw
{
    [RequireComponent(typeof(UILineRenderer))]
    public class RuneDrawManager : MonoBehaviour
    {
        [HideInInspector] public RuneVariation currentVariation;

        [SerializeField] private InputManager   inputManager;
        [SerializeField] private UILineRenderer lineRenderer;
        [SerializeField] private RuneRecognizer recognizer;


        [SerializeField] private float drawLineThickness = 0.02f;
        [SerializeField] private bool  showDrawPoints;

        [Header("Doesn't affects already created runes\nplease recreate them to apply changes")]
        [SerializeField] private float distanceBetweenPoints = 0.02f;
        [SerializeField] private float acceptableError = 0.001f;
        [SerializeField] private float heavyCheckStep  = 0.005f;

        private readonly List<Vector2> drawPoints = new();
        private readonly float screenWidth = Screen.width; // Probably reducing amount of Screen calls is worth it idk

        private Rect    drawFrame;
        private Vector2 lastPoint;
        private Vector2 momentSum = Vector2.zero;

        private void Start()
        {
            lineRenderer.thickness = drawLineThickness * Screen.width;
            Gizmos.color = Color.green;
        }

        private void OnEnable()
        {
            inputManager.OnDrawStart += HandleDrawStart;
            inputManager.OnDrawEnd += HandleDrawEnd;
        }

        private void OnDisable()
        {
            inputManager.OnDrawStart -= HandleDrawStart;
            inputManager.OnDrawEnd -= HandleDrawEnd;
        }


        private void OnDrawGizmos()
        {
            if (!showDrawPoints) return;

            foreach (Vector2 point in drawPoints)
            {
                Gizmos.DrawSphere(point * screenWidth, distanceBetweenPoints * screenWidth / 2);
            }
        }

        public event Action<RuneVariation> OnRuneDrawn;


        private void HandleDrawStart(Vector2 nextDrawPosition)
        {
            inputManager.OnNextDrawPosition += HandleNextDrawPosition;
            lineRenderer.points.Clear();
            drawPoints.Clear();
            momentSum = Vector2.zero;
            drawFrame = new Rect(nextDrawPosition.x, nextDrawPosition.y, 0, 0);
            CreateNewPoint(nextDrawPosition);
            lineRenderer.SetAllDirty();
        }

        private void HandleNextDrawPosition(Vector2 nextDrawPosition)
        {
            lineRenderer.points.Add(nextDrawPosition * screenWidth);
            lineRenderer.SetAllDirty();

            while ((lastPoint - nextDrawPosition).magnitude >= distanceBetweenPoints)
                CreateNewPoint(lastPoint + (nextDrawPosition - lastPoint).normalized * distanceBetweenPoints);
        }

        private void HandleDrawEnd()
        {
            inputManager.OnNextDrawPosition -= HandleNextDrawPosition;
            PrepareRuneVariation();
            lineRenderer.points.Clear();
            lineRenderer.SetAllDirty();

            recognizer.Recognize(currentVariation);
            OnRuneDrawn?.Invoke(currentVariation);
        }

        private void CreateNewPoint(Vector2 position)
        {
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
            currentVariation = new RuneVariation
            {
                points = new Vector2[drawPoints.Count],
                height = drawFrame.height / drawFrame.width
            };

            Vector2 ratioFactor = new(1, currentVariation.height);
            currentVariation.massCenter = Rect.PointToNormalized(drawFrame, momentSum / drawPoints.Count) * ratioFactor;

            for (int i = 0; i < drawPoints.Count; i++)
            {
                currentVariation.points[i] = Rect.PointToNormalized(drawFrame, drawPoints[i]) * ratioFactor;
            }
        }
    }
}