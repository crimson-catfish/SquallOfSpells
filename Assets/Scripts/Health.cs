using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    private float health;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
            Die();
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}