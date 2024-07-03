using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : Singleton<InputManager>
{
    public event Action<Vector2> OnMove;

    public event Action<Vector2> OnNextDrawPosition;
    public event Action<Vector2> OnDrawStart;
    public event Action OnDrawEnd;

    public event Action OnDeleteRune;
    public event Action OnNewRune;
    public event Action OnAddVariation;
    public event Action OnSelectRecognized;

    private EventSystem eventSystem;
    private Controls controls;
    private readonly float screenWidth = Screen.width;
    private readonly List<Canvas> enabledCanvases = new();
    private Canvas[] allCanvases;


    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        controls.RuneCreatingUI.Enable();
#endif
        controls.Move.Enable();
        controls.Draw.Enable();
    }

    void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        allCanvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas.enabled && canvas.gameObject.layer == LayerMask.NameToLayer("UI"))
                enabledCanvases.Add(canvas);
        }

        SetMoveActions();
        SetDrawActions();
#if UNITY_EDITOR
        SetRuneCreatingUIActions();
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        controls.RuneCreatingUI.Disable();
#endif
        controls.Move.Enable();
        controls.Draw.Disable();
    }

        // public void SetControlsMap(InputActionMap map)
        // {
        //     controls.Touch.Disable();
        //     map.Enable();
        // }

    private bool IsOverAnyUI(Vector2 point)
    {
        PointerEventData pointerData;
        List<RaycastResult> results;

        foreach (Canvas canvas in enabledCanvases)
        {
            results = new List<RaycastResult>();

            pointerData = new PointerEventData(eventSystem) { position = point };

            GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();

            raycaster.Raycast(pointerData, results);

            if (results.Count > 0)
                return true;
        }

        return false;
    }

    private void SetMoveActions()
    {
        Controls.MoveActions actions = controls.Move;

        actions.Direction.performed += context =>
            OnMove?.Invoke(context.ReadValue<Vector2>());

        actions.Direction.canceled += _ =>
            OnMove?.Invoke(Vector2.zero);
    }

    private void SetDrawActions()
    {
        Controls.DrawActions actions = controls.Draw;

        actions.Contact.started += HandleDrawContactStart;

        actions.Position.performed += context =>
            OnNextDrawPosition?.Invoke(context.ReadValue<Vector2>() / screenWidth);

        actions.Contact.canceled += _ =>
            OnDrawEnd?.Invoke();
    }

    private void SetRuneCreatingUIActions()
    {
        Controls.RuneCreatingUIActions actions = controls.RuneCreatingUI;

        actions.DeleteRune.performed += _ => OnDeleteRune?.Invoke();
        actions.NewRune.performed += _ => OnNewRune?.Invoke();
        actions.AddVariation.performed += _ => OnAddVariation?.Invoke();
        actions.SelectRecognized.performed += _ => OnSelectRecognized?.Invoke();
    }

    private void HandleDrawContactStart(InputAction.CallbackContext _)
    {
        Vector2 startPositionPixels = controls.Draw.Position.ReadValue<Vector2>();
        if (IsOverAnyUI(startPositionPixels) == false)
            OnDrawStart?.Invoke(startPositionPixels / screenWidth);
    }
}