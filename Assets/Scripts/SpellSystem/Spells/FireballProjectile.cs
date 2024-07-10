using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float explotionRadius;

    private void Update()
    {
        this.transform.Translate(new Vector3(1, 0, 0) * (speed * Time.deltaTime));
    }
}