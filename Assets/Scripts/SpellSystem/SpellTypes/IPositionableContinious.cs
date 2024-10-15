using UnityEngine;

namespace SquallOfSpells.SpellSystem
{
    public interface IPositionableContinious : ICastable
    {
        public void StartCasting(Vector2 position);
        public void NewPosition(Vector2  position);
        public void Release();
    }
}