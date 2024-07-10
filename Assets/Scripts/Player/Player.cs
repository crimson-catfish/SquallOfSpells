using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private RuneRecognizer recognizer;
    [SerializeField] private SpellContainer spellContainer;


    [SerializeField] private float moveSpeed = 2f;


    private InputManager inputManager;
    private Vector3 moveDirection;
    private Rune lastRecognized;

    private void OnEnable()
    {
        inputManager = InputManager.instance;

        inputManager.OnMove += HandleNewMoveDirection;
        inputManager.OnAimDirectionChange += HandleAimDirectionChange;
        inputManager.OnAimCast += HandleAimCast;
    }

    private void HandleAimCast(Vector2 direction)
    {
        LookTo(direction);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (moveDirection.sqrMagnitude == 0f)
            return;
        
        LookTo(moveDirection);
        transform.position += moveDirection * Time.deltaTime;
    }

    private void HandleNewMoveDirection(Vector2 direction)
    {
        moveDirection = direction * moveSpeed;
    }

    private void HandleAimDirectionChange(Vector2 direction)
    {
        LookTo(direction);
    }

    private void LookTo(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void CastFireball()
    {
    }

    private void CastShield()
    {
    }
}