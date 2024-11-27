using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public class FireballCaster : MonoBehaviour, IClick
    {
        [SerializeField] private GameObject     fireballProjectile;


        public void Cast(Vector2 direction)
        {
            Instantiate(fireballProjectile, this.transform.position,
                Quaternion.Euler(
                    new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)));
        }
    }
}