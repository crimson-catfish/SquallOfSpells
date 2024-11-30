using UnityEngine;

namespace SquallOfSpells.Utility
{
    public class PerformanceManger : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate;

        private void Awake()
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
}