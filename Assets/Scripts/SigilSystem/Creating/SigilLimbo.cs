#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SquallOfSpells.SigilSystem.Creating
{
    public class SigilLimbo : MonoBehaviour
    {
        [FormerlySerializedAs("runesToDelete")] public List<Sigil> sigilsToDelete = new();
    }
}
#endif