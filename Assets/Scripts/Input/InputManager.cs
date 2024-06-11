using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : Singleton<InputManager>
{
    public event Action<Vector2> OnNextDrawPosition;
    public event Action<Vector2> OnDrawStart;
    public event Action OnDrawEnd;

    private EventSystem eventSystem;
    private Controls controls;
    private readonly float screenWidth = Screen.width;
    private List<Canvas> enabledCanvases = new();
    private Canvas[] allCanvases;

    private void Awake()
    {
        controls = new Controls();
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

        controls.Touch.DrawContact.started += _ =>
        {
            Vector2 startPositionPixels = controls.Touch.DrawPosition.ReadValue<Vector2>();
            if (IsOverAnyUI(startPositionPixels) == false)
                OnDrawStart?.Invoke(startPositionPixels / screenWidth);
        };

        controls.Touch.DrawPosition.performed += context =>
            OnNextDrawPosition?.Invoke(context.ReadValue<Vector2>() / screenWidth);

        controls.Touch.DrawContact.canceled += _ =>
            OnDrawEnd?.Invoke();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
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
            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit " + result.gameObject.name);
            }
            if (results.Count > 0)
                return true;
        }

        return false;
    }
}