using SquallOfSpells.SigilSystem;
using SquallOfSpells.SpellSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace SquallOfSpells
{
    [RequireComponent(typeof(Health))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private     InputManager    inputManager;
        [SerializeField] private     SigilRecognizer recognizer;
        [SerializeField] private new Rigidbody2D     rigidbody;

        [SerializeField] private float moveSpeed = 2f;

        private Sigil   lastRecognized;
        private Vector2 moveDirection;

        private void OnEnable()
        {
            inputManager.OnMove += HandleNewMoveDirection;
        }

        private void FixedUpdate()
        {
            rigidbody.velocity = moveDirection;
        }

        private void HandleNewMoveDirection(Vector2 direction)
        {
            moveDirection = direction * moveSpeed;
        }
    }
}