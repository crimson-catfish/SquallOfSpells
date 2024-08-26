using SquallOfSpells.Plugins;
using UnityEngine;
using UnityEngine.Serialization;

namespace SquallOfSpells.SpellSystem
{
    public class ShotgunCaster : MonoBehaviour, IAimable
    {
        [SerializeField] private int   numberOfBullets;
        [SerializeField] private float spreadAngle;
        [SerializeField] private float spreadHorizontal;
        [SerializeField] private float spreadAlong;

        [SerializeField] private GameObject shotgunProjectile;


        public void Cast(Vector2 direction)
        {
            for (int i = 0; i < numberOfBullets; i++)
            {
                Vector2 bulletDirection =
                    Quaternion.Euler(0, 0, RandomGaussian.Generate(-spreadAngle, spreadAngle)) *
                    direction;

                Quaternion rotation = Quaternion.Euler(
                    new Vector3(0, 0, Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg));

                Vector3 offsetX = direction.normalized *
                                  RandomGaussian.Generate(-spreadHorizontal, spreadHorizontal);

                Vector3 offsetY =
                    new Vector2(direction.y, direction.x).normalized * // direction vector rotated by 90 degrees
                    RandomGaussian.Generate(-spreadAlong, spreadAlong);

                Vector3 position = this.transform.position + offsetX + offsetY;

                Instantiate(shotgunProjectile, position, rotation);
            }
        }
    }
}