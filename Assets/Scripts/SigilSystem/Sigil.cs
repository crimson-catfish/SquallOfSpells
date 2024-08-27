using System;
using UnityEngine;

namespace SquallOfSpells.SigilSystem
{
    /// <summary>
    ///     width always 1, height is positive.
    /// </summary>
    [Serializable]
    public class Sigil
    {
        public const float width = 1;
        public       float height;

        public Vector2 massCenter = Vector2.zero;

        public Vector2[] points;
    }
}