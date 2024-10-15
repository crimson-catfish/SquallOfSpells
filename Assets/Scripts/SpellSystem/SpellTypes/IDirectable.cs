using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public interface IDirectable : ICastable
    {
        public void Cast(Vector2 direction);
    }
}