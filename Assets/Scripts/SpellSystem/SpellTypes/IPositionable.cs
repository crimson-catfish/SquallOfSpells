using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public interface IPositionable : ICastable
    {
        public void Cast(Vector2 position);
    }
}