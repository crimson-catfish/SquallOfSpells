using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public interface IAimable
    {
        public void AimInit();
        public void Cast(Vector2 direction);
    }
}