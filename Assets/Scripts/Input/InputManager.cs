using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> OnNextDrawPosition;
    public event Action OnDrawSubmit;

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
        controls.Touch.Draw.started += (context) => Debug.Log("strt");
        controls.Touch.DrawSubmit.performed += DrawSubmited;
    }


    private void Draw(InputAction.CallbackContext context)
    {
        OnNextDrawPosition?.Invoke(context.ReadValue<Vector2>());
    }

    private void DrawSubmited(InputAction.CallbackContext context)
    {
        OnDrawSubmit?.Invoke();
    }
}
