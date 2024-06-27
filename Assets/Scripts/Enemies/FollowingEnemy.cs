using UnityEngine;

public abstract class FollowingEnemy : Enemy
{
    [SerializeField] private float speed;

    protected void Follow()
    {
        Vector3 position = transform.position;

        Vector3 direction = player.transform.position - position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (direction.magnitude > 1f)
            position += direction.normalized * (speed * Time.deltaTime);

        transform.position = position;
    }
}