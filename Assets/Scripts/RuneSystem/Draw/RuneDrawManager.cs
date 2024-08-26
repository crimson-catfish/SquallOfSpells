using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SquallOfSpells.RuneSystem.Draw
{
    [RequireComponent(typeof(UILineRenderer))]
    public class RuneDrawManager : MonoBehaviour
    {
        public RuneVariation currentVariation;

        [SerializeField] private InputManager    inputManager;
        [SerializeField] private UILineRenderer  lineRenderer;
        [SerializeField] private RuneRecognizer  recognizer;
        [SerializeField] private TextMeshProUGUI drawPositionsDisplayer;


        [SerializeField] private float drawLineThickness = 0.02f;
        [SerializeField] private bool  showDrawPoints;
        [SerializeField] private bool  showDrawPositionsCount;


        [Header("Doesn't affects already created runes\nplease recreate them to apply changes")]
        [SerializeField] private float distanceBetweenPoints = 0.02f;

        private readonly List<Vector2> drawPositions = new();

        private Rect drawFrame;

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

            foreach (Vector2 point in currentVariation.points)
                Gizmos.DrawSphere(point * Screen.width, distanceBetweenPoints * Screen.width / 2);
        }

        public event Action<RuneVariation> OnRuneDrawn;


        private void HandleDrawStart(Vector2 nextDrawPosition)
        {
            drawPositions.Clear();
            inputManager.OnNextDrawPosition += HandleNextDrawPosition;
            drawFrame = new Rect(nextDrawPosition.x, nextDrawPosition.y, 0, 0);

            lineRenderer.points.Clear();
            lineRenderer.SetAllDirty();
        }

        private void HandleNextDrawPosition(Vector2 position)
        {
            drawPositions.Add(position);

            if (position.x > drawFrame.xMax) drawFrame.xMax = position.x;
            if (position.x < drawFrame.xMin) drawFrame.xMin = position.x;
            if (position.y > drawFrame.yMax) drawFrame.yMax = position.y;
            if (position.y < drawFrame.yMin) drawFrame.yMin = position.y;

            lineRenderer.points.Add(position);
            lineRenderer.SetAllDirty();
        }

        private void HandleDrawEnd()
        {
            inputManager.OnNextDrawPosition -= HandleNextDrawPosition;

            lineRenderer.points.Clear();
            lineRenderer.SetAllDirty();

            if (drawPositions.Count < 2)
                return;

            CreateRuneVariation();

            recognizer.Recognize(currentVariation);
            OnRuneDrawn?.Invoke(currentVariation);

            if (showDrawPositionsCount)
                drawPositionsDisplayer.text = drawPositions.Count.ToString();
        }

        private void CreateRuneVariation()
        {
            List<Vector2> drawPoints = new();
            Vector2 lastPoint = MapToFrame(drawPositions[0]);
            Vector2 momentum = Vector2.zero;


            foreach (Vector2 positionScreen in drawPositions)
            {
                Vector2 position = MapToFrame(positionScreen);

                while ((lastPoint - position).magnitude >= distanceBetweenPoints)
                {
                    Vector2 point = lastPoint + (position - lastPoint).normalized * distanceBetweenPoints;
                    lastPoint = point;
                    drawPoints.Add(point);
                    momentum += point;
                }
            }

            currentVariation = new RuneVariation
            {
                points = drawPoints.ToArray(),
                height = drawFrame.height / drawFrame.width,
                massCenter = momentum / drawPoints.Count
            };
        }

        Vector2 MapToFrame(Vector2 vector2)
        {
            return new Vector2
            (
                (vector2.x - drawFrame.xMin) / drawFrame.width,
                (vector2.y - drawFrame.yMin) / drawFrame.width
            );
        }
    }
}