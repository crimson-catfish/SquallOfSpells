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
        controls.Touch.IsDrawing.started += _ => OnDrawStart?.Invoke(); // somehow called only after OnNextDrawPosition
        controls.Touch.Draw.performed += context => OnNextDrawPosition?.Invoke(context.ReadValue<Vector2>() / new Vector2(Screen.width, Screen.height));
        controls.Touch.IsDrawing.canceled += _ => OnDrawEnd?.Invoke();
        controls.Touch.Cast.performed += _ => OnCast?.Invoke();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
