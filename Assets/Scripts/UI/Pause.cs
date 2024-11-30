using UnityEngine;

namespace SquallOfSpells.UI
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] private CanvasManager canvasManager;
        [SerializeField] private Canvas        inGameUI;
        [SerializeField] private Canvas        pauseMenuUI;


        public void PauseGame()
        {
            Time.timeScale = 0;
            canvasManager.DisableCanvas(inGameUI);
            canvasManager.EnableCanvas(pauseMenuUI);
        }
    }
}