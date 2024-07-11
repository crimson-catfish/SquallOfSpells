using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private InputSettings settings;


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
    private Transform _playerTransform;
    private Camera _mainCamera;

    public Controls Controls { get; private set; }

    private Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;

            return _mainCamera;
        }
    }

    private Vector2 PlayerScreenPosition
    {
        get
        {
            if (_playerTransform == null)
                _playerTransform = GameObject.FindWithTag("Player").transform;

            return MainCamera.WorldToScreenPoint(_playerTransform.position);
        }
    }

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

        TraceHandler("Draw contact start");

        OnDrawStart?.Invoke(startPositionPixels / screenWidth);

        Controls.Draw.Contact.canceled += HandleDrawContactEnd;
    }

    private void HandleDrawContactEnd(InputAction.CallbackContext _)
    {
        TraceHandler("Draw contact end");


        OnDrawEnd?.Invoke();
        Controls.Draw.Contact.canceled -= HandleDrawContactEnd;
    }

    private void HandleAimContactStart(InputAction.CallbackContext _)
    {
        Vector2 startPositionPixels = Controls.Aim.Position.ReadValue<Vector2>();
        if (IsOverAnyUI(startPositionPixels))
            return;
        
        TraceHandler("Aim contact start");

        if (settings.origin == InputSettings.AimOrigin.Free)
            aimStartPosition = startPositionPixels;

        Controls.Aim.Position.performed += HandleAimPosition;
        Controls.Aim.Contact.canceled += HandleAimPressed;
    }

    private void HandleAimPosition(InputAction.CallbackContext context)
    {
        TraceHandler("Aim position");


        Vector2 position = context.ReadValue<Vector2>();

        if ((position - aimStartPosition).sqrMagnitude > math.pow(settings.stickDeadzone * screenWidth, 2))
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
        TraceHandler("Aim pressed");

        OnAimCast?.Invoke(Controls.Aim.Position.ReadValue<Vector2>() - PlayerScreenPosition);

        Controls.Aim.Position.performed -= HandleAimPosition;

        SwitchToActionMap(Controls.Draw);
    }

    private void HandleAimDirectionChange(InputAction.CallbackContext context)
    {
        TraceHandler("Aim direction change");

        Vector2 newDirection = default;

        if (settings.origin == InputSettings.AimOrigin.Free)
            newDirection = context.ReadValue<Vector2>() - aimStartPosition;
        else if (settings.origin == InputSettings.AimOrigin.Player)
            newDirection = context.ReadValue<Vector2>() - PlayerScreenPosition;

        if (settings.direction == InputSettings.AimDirection.Reverse)
            newDirection *= -1f;

        OnAimDirectionChange?.Invoke(newDirection);
    }

    private void HandleAimDirectionUnleash(InputAction.CallbackContext _)
    {
        TraceHandler("Aim unleash");

        Vector2 direction = default;

        if (settings.origin == InputSettings.AimOrigin.Free)
            direction = Controls.Aim.Position.ReadValue<Vector2>() - aimStartPosition;
        else if (settings.origin == InputSettings.AimOrigin.Player)
            direction = Controls.Aim.Position.ReadValue<Vector2>() - PlayerScreenPosition;

        if (settings.direction == InputSettings.AimDirection.Reverse)
            direction *= -1f;

        OnAimCast?.Invoke(direction);

        Controls.Aim.Position.performed -= HandleAimDirectionChange;
        Controls.Aim.Contact.canceled -= HandleAimDirectionUnleash;

        SwitchToActionMap(Controls.Draw);
    }

    private void TraceHandler(string message)
    {
        if (settings.traceHandlers)
            Debug.Log(message);
    }
}