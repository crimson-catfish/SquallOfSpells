using System;
using UnityEngine;

namespace SquallOfSpells
{
    public class DirectableSpellsContainer : MonoBehaviour
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
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}