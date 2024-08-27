#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace SquallOfSpells.SigilSystem.RuneCreating
{
    public class RuneLimbo : MonoBehaviour
    {
        public List<Rune> runesToDelete = new();
    }
}
#endif