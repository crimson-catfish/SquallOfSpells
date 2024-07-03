using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float explotionRadius;


    private void Update()
    {
        Move();
    }

    private void Move()
    {
        GetComponent<Rigidbody2D>().velocity = transform.forward * speed;
    }
}