using UnityEngine;

namespace SquallOfSpells.UI
{
    public class Resume : MonoBehaviour
    {
        [SerializeField] private CanvasManager canvasManager;
        [SerializeField] private Canvas        inGameUI;
        [SerializeField] private Canvas        pauseMenuUI;

        public void ResumeGame()
        {
            Time.timeScale = 1;
            canvasManager.DisableCanvas(pauseMenuUI);
            canvasManager.EnableCanvas(inGameUI);
        }
    }
}