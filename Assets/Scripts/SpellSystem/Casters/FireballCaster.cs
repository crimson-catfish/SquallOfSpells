using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public class FireballCaster : MonoBehaviour, IAimable
    {
        [SerializeField] private GameObject     fireballProjectile;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void AimInit()
        {
            spriteRenderer.enabled = true;
        }

        public void Cast(Vector2 direction)
        {
            Instantiate(fireballProjectile, this.transform.position,
                Quaternion.Euler(
                    new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)));

            spriteRenderer.enabled = false;
        }
    }
}