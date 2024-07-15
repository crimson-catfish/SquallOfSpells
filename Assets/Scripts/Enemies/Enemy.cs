using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected float startHealth;

    public float Health => health;

    protected GameObject player;

    private float health;

    protected virtual void OnEnable()
    {
        health = startHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

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