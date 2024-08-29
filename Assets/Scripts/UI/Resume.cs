using UnityEngine;

namespace SquallOfSpells.UI
{
    public class Resume : MonoBehaviour
    {
        [SerializeField] private Canvas inGameUI;
        [SerializeField] private Canvas pauseMenuUI;

        public void ResumeGame()
        {
            Time.timeScale = 1;
            CanvasManager.DisableCanvas(pauseMenuUI);
            CanvasManager.EnableCanvas(inGameUI);
        }
    }
}