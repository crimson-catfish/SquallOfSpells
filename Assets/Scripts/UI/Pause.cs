using UnityEngine;
using UnityEngine.Serialization;

namespace SquallOfSpells.UI
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] private Canvas inGameUI;
        [SerializeField] private Canvas pauseMenuUI;


        public void PauseGame()
        {
            Time.timeScale = 0;
            CanvasManager.DisableCanvas(inGameUI);
            CanvasManager.EnableCanvas(pauseMenuUI);
        }
    }
}