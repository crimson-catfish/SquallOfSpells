using UnityEngine;
using UnityEngine.Serialization;

namespace SquallOfSpells.SpellSystem
{
    public class DashSpell : MonoBehaviour, ICastable
    {
        [FormerlySerializedAs("player"), SerializeField] private PlayerMovement playerMovement;

        public void Cast()
        {
            playerMovement.Dash();
        }
    }
}