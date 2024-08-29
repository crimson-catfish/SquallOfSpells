using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SquallOfSpells
{
    [CreateAssetMenu(fileName = "Canvas manager", menuName = "Scriptable objects/Canvas manager")]
    public class CanvasManager : ScriptableObject
    {
        public static List<Canvas> uiCanvases = new();

        private void OnEnable()
        {
            SceneManager.sceneLoaded += (_, _) => UpdateUICanvasesList();
        }


        public static void EnableCanvas(Canvas canvas)
        {
            canvas.enabled = true;

            if (canvas.gameObject.layer == LayerMask.NameToLayer("UI"))
                uiCanvases.Add(canvas);
        }

        public static void DisableCanvas(Canvas canvas)
        {
            canvas.enabled = false;

            if (canvas.gameObject.layer == LayerMask.NameToLayer("UI"))
                uiCanvases.Remove(canvas);
        }

        public static void UpdateUICanvasesList()
        {
            Canvas[] allCanvases = FindObjectsOfType<Canvas>();

            foreach (Canvas canvas in allCanvases)
            {
                if (canvas.enabled && canvas.gameObject.layer == LayerMask.NameToLayer("UI"))
                    uiCanvases.Add(canvas);
            }
        }
    }
}