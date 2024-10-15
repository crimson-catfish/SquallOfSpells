using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private float damage;

        protected void DealDamage(GameObject hit)
        {
            if (hit.TryGetComponent(out Health healthComponent))
                healthComponent.TakeDamage(damage);
        }
    }
}