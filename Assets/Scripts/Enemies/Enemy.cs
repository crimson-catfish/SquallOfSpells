using UnityEngine;

namespace SquallOfSpells
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Health))]
    public abstract class Enemy : MonoBehaviour
    {
        protected GameObject player;


        protected virtual void OnEnable()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Die()
        {
            Destroy(this.gameObject);
        }
    }
}