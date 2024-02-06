using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] private float distanceBetweenPoints;
    private InputManager inputManager;
    private List<Vector2> drawPointsPositions = new();
    private Vector2 lastPointPosition;
    private int mass = 0;
    private Vector2 momentSum = Vector2.zero;
    private float[] drawFrame;


    private void Awake()
    {
        inputManager = InputManager.instance;
    }

    private void OnEnable()
    {
        inputManager.OnNextDrawPosition += HandleNextDrawPosition;
        inputManager.OnDrawEnd += HandleDrawEnd;
        inputManager.OnCast += HandleCast;
    }

    private void OnDisable()
    {
        inputManager.OnNextDrawPosition -= HandleNextDrawPosition;
        inputManager.OnDrawEnd -= HandleDrawEnd;
        inputManager.OnCast -= HandleCast;
    }


    private void HandleNextDrawPosition(Vector2 position)
    {
        // check if distance between previous and current position is too small
        if (drawPointsPositions.Count > 0 && (lastPointPosition - position).magnitude < distanceBetweenPoints) return;

        // check distances between all positions and current one
        foreach (Vector2 pointPosition in drawPointsPositions)
        {
            if ((pointPosition - position).magnitude < distanceBetweenPoints) return;
        }

        // create new point
        lastPointPosition = position;
        drawPointsPositions.Add(position);
        mass ++;
        momentSum += position;
        if (drawPointsPositions.Count == 1) drawFrame = new float[] { position.x, position.y, position.x, position.y };
        if (position.x < drawFrame[0]) drawFrame[0] = position.x;
        if (position.y < drawFrame[1]) drawFrame[1] = position.y;
        if (position.x > drawFrame[2]) drawFrame[2] = position.x;
        if (position.y > drawFrame[3]) drawFrame[3] = position.y;
    }

    private void HandleDrawEnd()
    {
        Vector2 massCenter = momentSum / mass;
        float ratio = (drawFrame[2] - drawFrame[0]) / (drawFrame[3] - drawFrame[1]);
        // recognize rune lol
        drawPointsPositions.Clear();
    }

    private void HandleCast()
    {
        // cast
    }
}