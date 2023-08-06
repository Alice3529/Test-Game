using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestTask.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] Canvas loseCanvas;
        [SerializeField] Canvas winCanvas;

        public static UIManager uIManager;

        private void Awake()
        {
            if (uIManager != null)
            {
                Destroy(uIManager);
            }
            uIManager = this;
        }

        public void ActiveWinCanvas()
        {
            winCanvas.enabled = true;
            Time.timeScale = 0f;
        }

        public void ActiveLoseCanvas()
        {
            loseCanvas.enabled = true;
            Time.timeScale = 0f;
        }

        public void Reload()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}
