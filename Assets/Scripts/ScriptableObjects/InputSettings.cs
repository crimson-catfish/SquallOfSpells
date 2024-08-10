using UnityEngine;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Input settings", menuName = "Scriptable objects/Input settings")]
    public class InputSettings : ScriptableObject
    {
        public enum AimDirection
        {
            Direct,
            Reverse
        }

        public enum AimOrigin
        {
            Player,
            Free
        }

        public float stickDeadzone = 0.02f;

        public AimDirection direction;
        public AimOrigin    origin;
    }
}