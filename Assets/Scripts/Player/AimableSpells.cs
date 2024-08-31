using System;
using UnityEngine;

namespace SquallOfSpells
{
    public class AimableSpells : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;


        private void OnEnable()
        {
            inputManager.OnAimDirectionChange += HandleAimDirectionChange;
        }

        private void OnDisable()
        {
            inputManager.OnAimDirectionChange -= HandleAimDirectionChange;
        }

        private void HandleAimDirectionChange(Vector2 direction)
        {
            // this.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y));
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}