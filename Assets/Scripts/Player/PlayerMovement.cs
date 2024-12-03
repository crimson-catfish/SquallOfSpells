using System.Collections;
using SquallOfSpells.SigilSystem;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Serialization;

namespace SquallOfSpells
{
    [RequireComponent(typeof(Health))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private     InputManager    inputManager;
        [SerializeField] private     SigilRecognizer recognizer;
        [SerializeField] private new Rigidbody2D     rigidbody;

        [SerializeField] private float moveSpeed       = 2f;
        [SerializeField] private float dashSpeed       = 8f;
        [SerializeField] private float dashTimeSeconds = 0.1f;

        private bool    isDashing;
        private Vector2 inputDirection;
        private Vector2 lastInputDirection = Vector2.right;
        private float   dashStartTime;

        private void FixedUpdate()
        {
            if (isDashing)
            {
                if (Time.time - dashStartTime > dashTimeSeconds)
                {
                    gameObject.layer = LayerMask.NameToLayer("Player");
                    isDashing = false;

                    return;
                }

                rigidbody.velocity = lastInputDirection * dashSpeed;
            }
            else
            {
                rigidbody.velocity = inputDirection * moveSpeed;
            }
        }

        private void OnEnable()
        {
            inputManager.OnMove += HandleNewMoveDirection;
        }

        private void HandleNewMoveDirection(Vector2 direction)
        {
            if (direction == Vector2.zero)
                lastInputDirection = inputDirection;

            inputDirection = direction;
        }

        public void Dash()
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerDashing");
            isDashing = true;
            dashStartTime = Time.time;
        }
    }
}