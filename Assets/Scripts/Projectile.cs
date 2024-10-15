using SquallOfSpells.SpellSystem;
using UnityEngine;

namespace SquallOfSpells
{
    public class Projectile : DamageDealer
    {
        [SerializeField] private string ownerTag;
        [SerializeField] private float  speed;


        private void Update()
        {
            RaycastHit2D hit = CheckForHit();

            if (hit.collider is not null && !hit.collider.gameObject.CompareTag(ownerTag))
            {
                DealDamage(hit.transform.gameObject);
                Destroy(this.gameObject);
            }

            Move();
        }

        private RaycastHit2D CheckForHit()
        {
            return Physics2D.Raycast(this.transform.position, this.transform.right, speed * Time.deltaTime);
        }

        private void Move()
        {
            this.transform.Translate(Vector3.right * (speed * Time.deltaTime));
        }
    }
}