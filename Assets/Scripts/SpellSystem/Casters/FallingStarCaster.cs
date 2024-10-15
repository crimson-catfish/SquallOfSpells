using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public class FallingStarCaster : MonoBehaviour, IPositionable
    {
        [SerializeField] private GameObject fallingStar;


        public void Cast(Vector2 position)
        {
            Instantiate(fallingStar, position, Quaternion.identity);
        }
    }
}