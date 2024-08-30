using UnityEngine;

namespace Plugins
{
    public class PerformanceManger : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 90;
        }
    }
}