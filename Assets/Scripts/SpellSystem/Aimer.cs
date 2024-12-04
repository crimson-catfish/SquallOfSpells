using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public class Aimer : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        private static readonly string[] targetLayers = { "Enemy", "Breakable" };

        public Vector2 ClosestTarget(float range)
        {
            float closestDistanceSqr = float.MaxValue;
            Vector2 closestTarget = (Vector2)player.transform.position + Vector2.up;

            foreach (string targetLayer in targetLayers)
            {
                Collider2D[] targets =
                    Physics2D.OverlapCircleAll(player.transform.position, range, LayerMask.GetMask(targetLayer));

                foreach (Collider2D target in targets)
                {
                    Vector2 distance = player.transform.position - target.transform.position;

                    if (distance.sqrMagnitude < closestDistanceSqr)
                    {
                        closestDistanceSqr = distance.sqrMagnitude;
                        closestTarget = target.transform.position;
                    }
                }

                if (closestDistanceSqr != float.MaxValue)
                    return closestTarget;
            }

            return closestTarget;
        }
    }
}