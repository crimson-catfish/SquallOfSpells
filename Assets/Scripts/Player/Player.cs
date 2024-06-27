using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;


    private InputManager inputManager;
    private Vector3 moveDirection;

    private void OnEnable()
    {
        inputManager = InputManager.instance;

        inputManager.OnMove += HandleNewMoveDirection;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += moveDirection * Time.deltaTime;
    }

    private void HandleNewMoveDirection(Vector2 direction)
    {
        moveDirection = direction * moveSpeed;
        print(moveDirection);
    }
}