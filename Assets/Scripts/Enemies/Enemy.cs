using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected float startHealth;

    protected GameObject player;


    protected virtual void OnEnable()
    {
        Health = startHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    #region IDamageable Members

    public float Health { get; private set; }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0f)
            Die();
    }

    #endregion
}