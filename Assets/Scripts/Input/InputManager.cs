using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public event Action<Vector2> OnNextDrawPosition;

    public event Action<Vector2> OnDrawStart;
    public event Action OnDrawEnd;

    private Controls controls;
    private readonly float screenWidth = Screen.width;

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
        controls.Touch.DrawContact.started += _ => OnDrawStart?.Invoke(controls.Touch.DrawPosition.ReadValue<Vector2>() / screenWidth);
        controls.Touch.DrawPosition.performed += context => OnNextDrawPosition?.Invoke(context.ReadValue<Vector2>() / screenWidth);
        controls.Touch.DrawContact.canceled += _ => OnDrawEnd?.Invoke();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}