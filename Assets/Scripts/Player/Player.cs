using SquallOfSpells.RuneSystem;
using SquallOfSpells.SpellSystem;
using UnityEngine;

namespace SquallOfSpells
{
    [RequireComponent(typeof(Health))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private     InputManager   inputManager;
        [SerializeField] private     RuneRecognizer recognizer;
        [SerializeField] private new Rigidbody2D    rigidbody;
        [SerializeField] private     SpellContainer spellContainer;

        [SerializeField] private float moveSpeed = 2f;

        private Rune    lastRecognized;
        private Vector2 moveDirection;

        private void OnEnable()
        {
            inputManager.OnMove += HandleNewMoveDirection;
            inputManager.OnAimDirectionChange += HandleAimDirectionChange;
            inputManager.OnAimCast += HandleAimCast;
        }

        private void FixedUpdate()
        {
            rigidbody.velocity = moveDirection;

            if (moveDirection != Vector2.zero)
                LookDirection(moveDirection);
        }


        private void HandleAimCast(Vector2 direction)
        {
            LookDirection(direction);
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

        #region IDamageable Members

        public float Health { get; set; }

        public void TakeDamage(float damage)
        {
            Health -= damage;
        }

        #endregion
    }
}