using UnityEngine;

namespace SquallOfSpells.UI
{
    public class Resume : MonoBehaviour
    {
        [SerializeField] private GameObject inGameUI;
        [SerializeField] private GameObject pauseMenuUI;

        public void ResumeGame()
        {
            Time.timeScale = 1;
            inGameUI.SetActive(true);
            pauseMenuUI.SetActive(false);
        }
    }
}