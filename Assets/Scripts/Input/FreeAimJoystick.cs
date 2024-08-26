using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace SquallOfSpells
{
    [RequireComponent(typeof(Image))]
    public class FreeAimJoystick : MonoBehaviour
    {
        [SerializeField, Range(0f, 0.5f)] private float joystickRange = 0.44f;

        [SerializeField] private InputSettings settings;
        [SerializeField] private InputManager  inputManager;


        [SerializeField] private Image baseImage;

        [SerializeField] private GameObject handle;
        [SerializeField] private Image      handleImage;


        private void OnEnable()
        {
            inputManager.OnAimStart += HandleAimStart;
            inputManager.OnAimCast += HandleAimCast;
        }

        private void HandleAimStart(Vector2 startPosition)
        {
            if (settings.origin == InputSettings.AimOrigin.Free)
            {
                this.transform.position = startPosition;
                baseImage.enabled = true;
            }
            else if (settings.origin == InputSettings.AimOrigin.Player)
            {
            }

            handle.transform.position = Vector3.zero;
            handleImage.enabled = true;

            inputManager.OnAimDirectionChange += HandleAimDirectionChange;
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
}