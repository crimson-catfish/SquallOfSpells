using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private RuneRecognizer recognizer;
    [SerializeField] private SpellContainer spellContainer;
    [SerializeField] private new Rigidbody2D rigidbody;

    [SerializeField] private float moveSpeed = 2f;


    private Vector2 moveDirection;
    private Rune lastRecognized;

    private void OnEnable()
    {
        inputManager.OnMove += HandleNewMoveDirection;
        inputManager.OnAimDirectionChange += HandleAimDirectionChange;
        inputManager.OnAimCast += HandleAimCast;
    }

    private void HandleAimCast(Vector2 direction)
    {
        LookDirection(direction);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = moveDirection;

        if (moveDirection != Vector2.zero)
            LookDirection(moveDirection);
    }

    private void LookDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rigidbody.rotation = angle;
    }

    private void HandleNewMoveDirection(Vector2 direction)
    {
        moveDirection = direction * moveSpeed;
    }

    private void HandleAimDirectionChange(Vector2 direction)
    {
        LookDirection(direction);
    }
}