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
    }

    private void HandleRuneRecognition(Rune rune)
    {
        if (rune == null)
            return;

        // Spells spell = spellContainer.spells[rune];
        //
        // switch (spell)
        // {
        //     case Spells.Fireball:
        //         
        //         break;
        //     case Spells.Shield:
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException();
        // }
    }

    private void CastFireball()
    {
    }

    private void CastShield()
    {
    }
}