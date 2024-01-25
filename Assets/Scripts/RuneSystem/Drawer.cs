using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] private float distanceBetweenPoints;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject drawPoint;

    private List<Vector2> drawPointsPositions = new();
    private Vector2 lastPointPosition;


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
        if (transform.childCount > 0 && (lastPointPosition - position).magnitude < distanceBetweenPoints) return;

        // check distances between all positions and current one
        foreach (Vector2 pointPosition in drawPointsPositions)
        {
            if ((pointPosition - position).magnitude < distanceBetweenPoints) return;
        }

        // if reached this point - create new point
        Instantiate(drawPoint, new Vector3(position.x, position.y, 0f), Quaternion.identity);
        lastPointPosition = position;
        drawPointsPositions.Add(position);
    }

    private void HandleDrawEnd()
    {
        // recognize rune lol
        drawPointsPositions.Clear();
    }

    private void HandleCast()
    {
        // cast
    }    
}