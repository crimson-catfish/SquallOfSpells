using UnityEngine;
using UnityEngine.Serialization;

namespace SquallOfSpells.UI
{
    public class PauseManager : MonoBehaviour
    {
        [FormerlySerializedAs("InGameUI"), SerializeField] private GameObject inGameUI;

        public void PauseGame()
        {
            Time.timeScale = 0;
            inGameUI.SetActive(false);  
        }
    }
}