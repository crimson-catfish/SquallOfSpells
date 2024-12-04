using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public class FireballSpell : MonoBehaviour, ICastable
    {
        [SerializeField] private GameObject fireballProjectile;
        [SerializeField] private Aimer      aimer;

        [SerializeField] private float autoAimRange = 5.0f;


        public void Cast()
        {
            Vector2 direction = aimer.ClosestTarget(autoAimRange) - (Vector2)transform.position;

            Instantiate(fireballProjectile, this.transform.position,
                Quaternion.Euler(
                    new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)));
        }
    }
}