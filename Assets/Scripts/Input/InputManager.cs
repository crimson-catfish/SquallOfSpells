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
        controls.Touch.Draw.performed += Draw;
        controls.Touch.IsDrawing.canceled += DrawEnd;
        controls.Touch.Cast.performed += Cast;
    }


    private void Draw(InputAction.CallbackContext context)
    {
        OnNextDrawPosition?.Invoke(context.ReadValue<Vector2>());
    }

    private void DrawEnd(InputAction.CallbackContext context)
    {
        OnDrawEnd?.Invoke();
    }

    private void Cast(InputAction.CallbackContext context)
    {
        OnCast?.Invoke();
    }
}
