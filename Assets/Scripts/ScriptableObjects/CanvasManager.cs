using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Canvas manager", menuName = "Scriptable objects/Canvas manager")]
    public class CanvasManager : ScriptableObject
    {
        [SerializeField] private bool log;

        public readonly List<Canvas> uiCanvases = new();

        private void OnEnable()
        {
            if (log)
                Debug.Log("enabled");

            SceneManager.sceneLoaded += (_, _) => UpdateUICanvasesList();
        }


        public void EnableCanvas(Canvas canvas)
        {
            canvas.enabled = true;

            if (canvas.gameObject.layer == LayerMask.NameToLayer("UI"))
                uiCanvases.Add(canvas);
        }

        public void DisableCanvas(Canvas canvas)
        {
            canvas.enabled = false;

            if (canvas.gameObject.layer == LayerMask.NameToLayer("UI"))
                uiCanvases.Remove(canvas);
        }

        private void UpdateUICanvasesList()
        {
            uiCanvases.Clear();

            if (log)
                Debug.Log("updating canvases...");

            Canvas[] allCanvases = FindObjectsOfType<Canvas>();

            foreach (Canvas canvas in allCanvases)
            {
                if (canvas.enabled && canvas.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    uiCanvases.Add(canvas);

                    if (log)
                        Debug.Log(canvas);
                }
            }
        }
    }
}