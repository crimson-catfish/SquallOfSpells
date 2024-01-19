using System;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    

    private void Awake()
    {

    }

    private void OnEnable()
    {
        inputManager.OnNextDrawPosition += HandleNextDrawPosition;
        inputManager.OnDrawSubmit += HandleDrawSubmit;
    }

    private void OnDisable()
    {
        inputManager.OnNextDrawPosition -= HandleNextDrawPosition;
        inputManager.OnDrawSubmit -= HandleDrawSubmit;  
    }


    private void HandleNextDrawPosition(Vector2 vector)
    {
        Debug.Log(vector);
    }
    
    private void HandleDrawSubmit()
    {
        Debug.Log("ASDFHJ");
    }
}