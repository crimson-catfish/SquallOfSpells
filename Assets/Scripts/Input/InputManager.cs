using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : Singleton<InputManager>
{
    private float aimStickDeadzone = 0.02f;
    private bool traceHandlers = false;


    // RuneCreationGUI
    public event Action OnDeleteRune;
    public event Action OnNewRune;
    public event Action OnAddVariation;
    public event Action OnSelectRecognized;

    // Move
    public event Action<Vector2> OnMove;

    // Draw
    public event Action<Vector2> OnDrawStart;
    public event Action<Vector2> OnNextDrawPosition;
    public event Action OnDrawEnd;

    // Aim
    public event Action<Vector2> OnAimStart;
    public event Action<Vector2> OnAimDirectionChange;
    public event Action<Vector2> OnAimCast;

    private EventSystem eventSystem;
    private readonly float screenWidth = Screen.width;
    private readonly List<Canvas> enabledCanvases = new();
    private Canvas[] allCanvases;
    private InputActionMap currentMap;
    private Vector2 aimStartPosition;

    public Controls Controls { get; private set; }

    private void Awake()
    {
        Controls = new Controls();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        Controls.RuneCreatingUI.Enable();
#endif
        Controls.Move.Enable();
        Controls.Draw.Enable();
        currentMap = Controls.Draw;
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

#if UNITY_EDITOR
        SetRuneCreatingUIActions();
#endif
        SetMoveActions();
        SetDrawActions();
        SetAimActions();
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        Controls.RuneCreatingUI.Disable();
#endif
        Controls.Move.Enable();
        Controls.Draw.Disable();
    }

    public void SwitchToActionMap(InputActionMap map)
    {
        // print("switching to " + map.name);
        // print("previous map was " + currentMap.name);

        currentMap.Disable();
        map.Enable();
        currentMap = map;
    }

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

    private void SetRuneCreatingUIActions()
    {
        Controls.RuneCreatingUIActions actions = Controls.RuneCreatingUI;

        actions.DeleteRune.performed += _ => OnDeleteRune?.Invoke();
        actions.NewRune.performed += _ => OnNewRune?.Invoke();
        actions.AddVariation.performed += _ => OnAddVariation?.Invoke();
        actions.SelectRecognized.performed += _ => OnSelectRecognized?.Invoke();
    }

    private void SetMoveActions()
    {
        Controls.MoveActions actions = Controls.Move;

        actions.Direction.performed += context =>
            OnMove?.Invoke(context.ReadValue<Vector2>());

        actions.Direction.canceled += _ =>
            OnMove?.Invoke(Vector2.zero);
    }

    private void SetDrawActions()
    {
        Controls.DrawActions actions = Controls.Draw;

        actions.Contact.started += HandleDrawContactStart;

        actions.Position.performed += context =>
            OnNextDrawPosition?.Invoke(context.ReadValue<Vector2>() / screenWidth);

        actions.Contact.canceled += _ =>
            OnDrawEnd?.Invoke();
    }

    private void SetAimActions()
    {
        Controls.Aim.Contact.started += HandleAimContactStart;
    }

    private void HandleDrawContactStart(InputAction.CallbackContext _)
    {
        Vector2 startPositionPixels = Controls.Draw.Position.ReadValue<Vector2>();
        if (IsOverAnyUI(startPositionPixels))
            return;

        if (traceHandlers)
            Debug.Log("Draw contact start");

        OnDrawStart?.Invoke(startPositionPixels / screenWidth);
    }

    private void HandleAimContactStart(InputAction.CallbackContext _)
    {
        Vector2 startPositionPixels = Controls.Aim.Position.ReadValue<Vector2>();
        if (IsOverAnyUI(startPositionPixels))
            return;

        if (traceHandlers)
            Debug.Log("Aim contact start");

        aimStartPosition = startPositionPixels;

        Controls.Aim.Position.performed += HandleAimPosition;
        Controls.Aim.Contact.canceled += HandleAimPressed;
    }

    private void HandleAimPosition(InputAction.CallbackContext context)
    {
        if (traceHandlers)
            Debug.Log("Aim position");

        Vector2 position = context.ReadValue<Vector2>();

        if ((position - aimStartPosition).sqrMagnitude > math.pow(aimStickDeadzone * screenWidth, 2))
        {
            OnAimStart?.Invoke(aimStartPosition);
            OnAimDirectionChange?.Invoke(position);
            Controls.Aim.Position.performed -= HandleAimPosition;
            Controls.Aim.Position.performed += HandleAimDirectionChange;
            Controls.Aim.Contact.canceled -= HandleAimPressed;
            Controls.Aim.Contact.canceled += HandleAimDirectionUnleash;
        }
    }

    private void HandleAimPressed(InputAction.CallbackContext context)
    {
        if (traceHandlers)
            Debug.Log("Aim pressed");

        GameObject player = GameObject.FindWithTag("Player");
        if (Camera.main == null || player == null)
            return;

        OnAimCast?.Invoke(Controls.Aim.Position.ReadValue<Vector2>() -
                          (Vector2)Camera.main.WorldToScreenPoint(player.transform.position));

        Controls.Aim.Position.performed -= HandleAimPosition;

        SwitchToActionMap(Controls.Draw);
    }

    private void HandleAimDirectionChange(InputAction.CallbackContext context)
    {
        if (traceHandlers)
            Debug.Log("Aim direction change");

        Vector2 newDirection = context.ReadValue<Vector2>() - aimStartPosition;

        OnAimDirectionChange?.Invoke(newDirection);
    }

    private void HandleAimDirectionUnleash(InputAction.CallbackContext _)
    {
        if (traceHandlers)
            Debug.Log("Aim unleash");

        OnAimCast?.Invoke(Controls.Aim.Position.ReadValue<Vector2>() - aimStartPosition);

        Controls.Aim.Position.performed -= HandleAimDirectionChange;
        Controls.Aim.Contact.canceled -= HandleAimDirectionUnleash;

        SwitchToActionMap(Controls.Draw);
    }
}