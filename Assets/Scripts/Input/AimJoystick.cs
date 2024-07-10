using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AimJoystick : MonoBehaviour
{
    [SerializeField, Range(0f, 0.5f)] private float joystickRange = 0.44f;

    [SerializeField] private Image baseImage;

    [SerializeField] private GameObject handle;
    [SerializeField] private Image handleImage;


    private void OnEnable()
    {
        InputManager.instance.OnAimStart += HandleAimStart;
        InputManager.instance.OnAimCast += HandleAimCast;
    }

    private void HandleAimStart(Vector2 startPosition)
    {
        this.transform.position = startPosition;
        baseImage.enabled = true;

        handle.transform.position = Vector3.zero;
        handleImage.enabled = true;

        InputManager.instance.OnAimDirectionChange += HandleAimDirectionChange;
    }

    private void HandleAimDirectionChange(Vector2 direction)
    {
        direction = direction.normalized * math.pow(direction.sqrMagnitude, joystickRange);

        handle.transform.position = this.transform.position + (Vector3)direction;
    }

    private void HandleAimCast(Vector2 _)
    {
        baseImage.enabled = false;
        handleImage.enabled = false;
    }
}