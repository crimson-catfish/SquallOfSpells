using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Logger = SquallOfSpells.Plugins.Logger;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Input manager", menuName = "Scriptable objects/Input manager")]
    public class InputManager : ScriptableObject
    {
        [SerializeField] private InputSettings settings;

        [SerializeField] private bool log;

        private readonly List<Canvas> enabledCanvases = new();
        private readonly float        screenWidth     = Screen.width;

        private Camera         _mainCamera;
        private Transform      _playerTransform;
        private Vector2        aimStartPosition;
        private Canvas[]       allCanvases;
        private InputActionMap currentMap;
        private EventSystem    eventSystem;
        private Logger         logger;

        public Controls Controls { get; private set; }

        private Camera MainCamera {
            get
            {
                if (_mainCamera == null)
                    _mainCamera = Camera.main;

                return _mainCamera;
            }
        }

        private Vector2 PlayerScreenPosition {
            get
            {
                if (_playerTransform == null)
                    _playerTransform = GameObject.FindWithTag("Player").transform;

                return MainCamera.WorldToScreenPoint(_playerTransform.position);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoad;
            Controls = new Controls();

            logger = new Logger(this, log);
        }

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
        public event Action          OnDrawEnd;

        // Aim
        public event Action<Vector2> OnAimStart;
        public event Action<Vector2> OnAimDirectionChange;
        public event Action<Vector2> OnAimCast;

        public void SwitchToActionMap(InputActionMap map)
        {
            currentMap.Disable();
            map.Enable();
            currentMap = map;
        }

        private void HandleSceneLoad(Scene scene, LoadSceneMode mode)
        {
#if UNITY_EDITOR
            if (scene.name == "RuneCreating")
            {
                Controls.RuneCreatingUI.Enable();
                SetRuneCreatingUIActions();
            }
#endif
            Controls.Move.Enable();
            Controls.Draw.Enable();
            currentMap = Controls.Draw;

            SetMoveActions();
            SetDrawActions();
            SetAimActions();

            UpdateCanvasesList();
        }

        private void UpdateCanvasesList()
        {
            eventSystem = FindObjectOfType<EventSystem>();
            allCanvases = FindObjectsOfType<Canvas>();

            foreach (Canvas canvas in allCanvases)
            {
                if (canvas.enabled && canvas.gameObject.layer == LayerMask.NameToLayer("UI"))
                    enabledCanvases.Add(canvas);
            }
        }

        private bool IsOverAnyUI(Vector2 point)
        {
            PointerEventData pointerData;
            List<RaycastResult> results;

            foreach (Canvas canvas in enabledCanvases)
            {
                results = new List<RaycastResult>();

                pointerData = new PointerEventData(eventSystem) { position = point };

                if (canvas.TryGetComponent<GraphicRaycaster>(out var raycaster) == false)
                    continue;

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

            logger.Log("Draw contact start");

            OnDrawStart?.Invoke(startPositionPixels / screenWidth);

            Controls.Draw.Contact.canceled += HandleDrawContactEnd;
        }

        private void HandleDrawContactEnd(InputAction.CallbackContext _)
        {
            logger.Log("Draw contact end");


            OnDrawEnd?.Invoke();
            Controls.Draw.Contact.canceled -= HandleDrawContactEnd;
        }

        private void HandleAimContactStart(InputAction.CallbackContext _)
        {
            Vector2 startPositionPixels = Controls.Aim.Position.ReadValue<Vector2>();

            if (IsOverAnyUI(startPositionPixels))
                return;

            logger.Log("Aim contact start");

            if (settings.origin == InputSettings.AimOrigin.Free)
                aimStartPosition = startPositionPixels;

            Controls.Aim.Position.performed += HandleAimPosition;
            Controls.Aim.Contact.canceled += HandleAimPressed;
        }

        private void HandleAimPosition(InputAction.CallbackContext context)
        {
            logger.Log("Aim position");


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
            logger.Log("Aim pressed");

            OnAimCast?.Invoke(Controls.Aim.Position.ReadValue<Vector2>() - PlayerScreenPosition);

            Controls.Aim.Position.performed -= HandleAimPosition;

            SwitchToActionMap(Controls.Draw);
        }

        private void HandleAimDirectionChange(InputAction.CallbackContext context)
        {
            logger.Log("Aim direction change");

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
            logger.Log("Aim unleash");

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
    }
}