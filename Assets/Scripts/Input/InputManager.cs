using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> OnNextDrawPosition;
    public event Action OnDrawEnd;
    public event Action OnCast;

    private Controls controls;


    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        controls.Touch.Draw.performed      += (context) => OnNextDrawPosition?.Invoke(context.ReadValue<Vector2>());
        controls.Touch.IsDrawing.canceled  += (context) => OnDrawEnd?.Invoke();
        controls.Touch.Cast.performed      += (context) => OnCast?.Invoke();
    }
}
