using System;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    
    List<Vector2> currentDraw;


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
        currentDraw.Add(position);
    }

    private void HandleDrawEnd()
    {
        // recognize rune lol
        currentDraw.Clear();
    }

    private void HandleCast()
    {
        // cast
    }    
}