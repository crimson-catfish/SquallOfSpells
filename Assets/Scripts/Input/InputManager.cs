using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public event Action<Vector2> OnNextDrawPosition;
    public event Action OnDrawStart;
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

    private void Start()
    {
        controls.Touch.IsDrawing.started    += (context) => OnDrawStart?.Invoke(); // somehow called only after OnNextDrawPosition
        controls.Touch.Draw.performed       += (context) => OnNextDrawPosition?.Invoke(context.ReadValue<Vector2>());
        controls.Touch.IsDrawing.canceled   += (context) => OnDrawEnd?.Invoke();
        controls.Touch.Cast.performed       += (context) => OnCast?.Invoke();
    }
    
    private void OnDisable()
    {
        controls.Disable();
    }
}
