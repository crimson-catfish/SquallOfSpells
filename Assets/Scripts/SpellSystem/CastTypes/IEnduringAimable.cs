using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public interface IEnduringAimable : IAimable
    {
        public void StartCasting(Vector2 direction);
        public void NewDirection(Vector2 direction);
    }
}