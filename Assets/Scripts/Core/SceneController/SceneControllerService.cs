using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CFD.Core
{
    /// <summary>
    /// Service for managing scene loading and transitions
    /// Provides both synchronous and asynchronous scene loading capabilities
    /// </summary>
    public class SceneControllerService : MonoBehaviour, ISceneController, IDisposable
    {
        [SerializeField] private float _minimumLoadTime = 0.5f;
        [SerializeField] private bool _useAsyncLoading = true;

        private bool _isLoading = false;

        public void Initialize()
        {
            Debug.Log("[SceneController] Initialized");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void Dispose()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Debug.Log("[SceneController] disposed");
        }

        public void LoadScene(int buildIndex)
        {
            if (_isLoading)
            {
                Debug.LogWarning($"[SceneController] Already loading a scene. Ignoring load request for: {buildIndex}");
                return;
            }

            if (buildIndex <= 0)
            {
                Debug.LogError("[SceneController] Cannot load scene with index lower than 1");
                return;
            }

            Debug.Log($"[SceneController] Loading scene: {buildIndex}");

            if (_useAsyncLoading)
            {
                LoadSceneAsyncCoroutine(buildIndex, null).Forget();
            }
            else
            {
                SceneManager.LoadScene(buildIndex);
            }
        }

        public void LoadSceneAsync(int buildIndex, Action onComplete = null)
        {
            if (_isLoading)
            {
                Debug.LogWarning($"[SceneController] Already loading a scene. Ignoring load request for: {buildIndex}");
                return;
            }

            if (buildIndex <= 0)
            {
                Debug.LogError("[SceneController] Cannot load scene with index lower than 1");
                return;
            }

            LoadSceneAsyncCoroutine(buildIndex, onComplete).Forget();
        }

        private async UniTask LoadSceneAsyncCoroutine(int buildIndex, Action onComplete)//TODO add cancellation token
        {
            if (!SceneExists(buildIndex))
            {
                Debug.LogError($"[SceneController] Scene with index {buildIndex} does not exist");
                return;
            }
            
            _isLoading = true;

            var startTime = Time.time;

            var asyncLoad = SceneManager.LoadSceneAsync(buildIndex);

            if (asyncLoad == null)
            {
                Debug.LogError($"[SceneController] Failed to load scene: {buildIndex}");
                _isLoading = false;
                return;
            }

            // Wait until the scene is fully loaded
            while (!asyncLoad.isDone)
            {
                // Progress goes from 0 to 0.9, then jumps to 1 when completed, so divide on 0.9 to get a smooth progress
                var progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

                await UniTask.Yield();
            }

            // Ensure minimum load time for smooth transitions
            var elapsedTime = Time.time - startTime;
            if (elapsedTime < _minimumLoadTime)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_minimumLoadTime - elapsedTime)).SuppressCancellationThrow();
            }

            _isLoading = false;

            Debug.Log($"[SceneController] Scene loaded: {buildIndex}");

            onComplete?.Invoke();
        }

        public void LoadMainMenu()
        {
            Debug.Log("[SceneController] Loading main menu");

            LoadScene(1);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //TODO add loading screen
            Debug.Log($"[SceneController] Scene loaded event: {scene.name}");
        }

        /// <summary>
        /// Checks if a scene exists in the build settings
        /// </summary>
        public bool SceneExists(int buildIndex)
        {
            if (buildIndex >= SceneManager.sceneCountInBuildSettings)
                return false;

            var scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);

            return !string.IsNullOrEmpty(scenePath);
        }
    }
}