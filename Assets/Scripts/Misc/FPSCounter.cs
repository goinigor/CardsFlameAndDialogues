using System;
using TMPro;
using UnityEngine;

namespace CFD.Misc
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text fpsText;
        [SerializeField] private float updateInterval = 0.5f;

        [Header("Color")] 
        [SerializeField] private Color goodFPSColor = Color.green;
        [SerializeField] private Color mediumFPSColor = Color.yellow;
        [SerializeField] private Color badFPSColor = Color.red;
        [SerializeField] private int goodFPSThreshold = 50;
        [SerializeField] private int mediumFPSThreshold = 30;

        private float elapsedTime;
        private int frameCount;
        private float currentFPS;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            elapsedTime += Time.unscaledDeltaTime;
            frameCount++;

            if (elapsedTime >= updateInterval)
            {
                currentFPS = frameCount / elapsedTime;

                if (fpsText != null)
                {
                    fpsText.text = $"FPS: {Mathf.RoundToInt(currentFPS)}";
                    UpdateFPSColor();
                }

                elapsedTime = 0f;
                frameCount = 0;
            }
        }

        private void UpdateFPSColor()
        {
            if (currentFPS >= goodFPSThreshold)
            {
                fpsText.color = goodFPSColor;
            }
            else if (currentFPS >= mediumFPSThreshold)
            {
                fpsText.color = mediumFPSColor;
            }
            else
            {
                fpsText.color = badFPSColor;
            }
        }

        // Public method to get current FPS if needed by other scripts
        public float GetCurrentFPS()
        {
            return currentFPS;
        }
    }
}