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
        [SerializeField] private CanvasManager canvasManager;

        [SerializeField] private bool log;

        private readonly float screenWidth = Screen.width;

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
        public event Action OnDeleteSigil;
        public event Action OnNewSigil;
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
                Controls.SigilCreatingUI.Enable();
                SetCreatingUIActions();
            }
#endif
            Controls.Move.Enable();
            Controls.Draw.Enable();
            currentMap = Controls.Draw;

            SetMoveActions();
            SetDrawActions();
            SetAimActions();
        }

        private bool IsOverUI(Vector2 point)
        {
            foreach (Canvas canvas in canvasManager.uiCanvases)
            {
                List<RaycastResult> results = new();

                PointerEventData pointerData = new(eventSystem) { position = point };

                if (canvas.TryGetComponent<GraphicRaycaster>(out var raycaster) == false)
                    continue;

                raycaster.Raycast(pointerData, results);

                if (results.Count > 0)
                    return true;
            }

            return false;
        }

        private void SetCreatingUIActions()
        {
            Controls.SigilCreatingUIActions actions = Controls.SigilCreatingUI;

            actions.Delete.performed += _ => OnDeleteSigil?.Invoke();
            actions.New.performed += _ => OnNewSigil?.Invoke();
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

            actions.Contact0.started += HandleContact0Start;
            actions.Contact1.started += HandleContact1Start;

            // actions.Contact0.started += _ => Debug.Log("Contact 0 started");
            // actions.Contact0.performed += _ => Debug.Log("Contact 0 performed");
            // actions.Contact0.canceled += _ => Debug.Log("Contact 0 canceled");
            //
            // actions.Contact1.started += _ => Debug.Log("Contact 1 started");
            // actions.Contact1.performed += _ => Debug.Log("Contact 1 performed");
            // actions.Contact1.canceled += _ => Debug.Log("Contact 1 canceled");
        }

        private void SetAimActions()
        {
            Controls.Aim.Contact.started += HandleAimContactStart;
        }

        private void HandleContact0Start(InputAction.CallbackContext _)
        {
            Vector2 startPositionPixels = Controls.Draw.Position0.ReadValue<Vector2>();

            if (IsOverUI(startPositionPixels))
                return;

            Controls.Draw.Position0.performed += HandleNewDrawPosition;

            HandleDrawContactStart(startPositionPixels);

            Controls.Draw.Contact0.canceled += HandleDrawContactEnd;
        }

        private void HandleContact1Start(InputAction.CallbackContext _)
        {
            Vector2 startPositionPixels = Controls.Draw.Position1.ReadValue<Vector2>();

            if (IsOverUI(startPositionPixels))
                return;

            Controls.Draw.Position1.performed += HandleNewDrawPosition;

            HandleDrawContactStart(startPositionPixels);

            Controls.Draw.Contact1.canceled += HandleDrawContactEnd;
        }

        private void HandleDrawContactStart(Vector2 startPositionPixels)
        {
            logger.Log("Draw contact start");

            OnDrawStart?.Invoke(startPositionPixels);

            Controls.Draw.Contact0.started -= HandleContact0Start;
            Controls.Draw.Contact1.started -= HandleContact1Start;
        }

        private void HandleNewDrawPosition(InputAction.CallbackContext context)
        {
            OnNextDrawPosition?.Invoke(context.ReadValue<Vector2>());
        }

        private void HandleDrawContactEnd(InputAction.CallbackContext _)
        {
            logger.Log("Draw contact end");

            OnDrawEnd?.Invoke();

            Controls.Draw.Position0.performed -= HandleNewDrawPosition;
            Controls.Draw.Position1.performed -= HandleNewDrawPosition;

            Controls.Draw.Contact0.started += HandleContact0Start;
            Controls.Draw.Contact1.started += HandleContact1Start;

            Controls.Draw.Contact0.canceled -= HandleDrawContactEnd;
            Controls.Draw.Contact1.canceled -= HandleDrawContactEnd;
        }

        private void HandleAimContactStart(InputAction.CallbackContext _)
        {
            Vector2 startPositionPixels = Controls.Aim.Position.ReadValue<Vector2>();

            if (IsOverUI(startPositionPixels))
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