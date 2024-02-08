using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public event Action<Vector2> OnNextDrawPosition;
    public event Action OnDrawStart;
    public event Action OnDrawEnd;
    public event Action OnCast;

    public event Action OnChooseRuneLeft;
    public event Action OnChooseRuneRight;

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
        controls.Touch.IsDrawing.started        += (context) => OnDrawStart?.Invoke();
        controls.Touch.Draw.performed           += (context) => OnNextDrawPosition?.Invoke(context.ReadValue<Vector2>());
        controls.Touch.IsDrawing.canceled       += (context) => OnDrawEnd?.Invoke();
        controls.Touch.Cast.performed           += (context) => OnCast?.Invoke();

        controls.Touch.ChooseRuneLeft.performed += (context) => OnChooseRuneLeft?.Invoke();
        controls.Touch.ChooseRuneLeft.performed += (context) => OnChooseRuneLeft?.Invoke();
    }
    
    private void OnDisable()
    {
        controls.Disable();
    }
}
