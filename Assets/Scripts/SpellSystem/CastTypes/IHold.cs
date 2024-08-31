using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public interface IHold
    {
        public void StartCasting(Vector2 direction);
        public void NewDirection(Vector2 direction);
        public void Release();
    }
}