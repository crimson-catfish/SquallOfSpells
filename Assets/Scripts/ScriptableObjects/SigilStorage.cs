using System.Collections.Generic;
using SquallOfSpells.SigilSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Sigil storage", menuName = "Scriptable objects/Sigil storage")]
    public class SigilStorage : ScriptableObject
    {
        [FormerlySerializedAs("Sigils")] public List<Sigil> sigils = new();

        private void OnEnable()
        {
            sigils.Clear();

            foreach (Sigil sigil in Resources.LoadAll<Sigil>("Sigils"))
                sigils.Add(sigil);
        }
    }
}