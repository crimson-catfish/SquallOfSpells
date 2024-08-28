using UnityEngine;

namespace SquallOfSpells
{
    public class GoofyEnemy : FollowingEnemy
    {
        private readonly Vector3 rotationSpeed = new(0f, 0f, 50f);

        private void Update()
        {
            Follow();
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
    }
}