using UnityEngine;
using UnityEngine.Serialization;

namespace SquallOfSpells.UI
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] private GameObject inGameUI;
        [SerializeField] private GameObject pauseMenuUI;


        public void PauseGame()
        {
            Time.timeScale = 0;
            inGameUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }
}