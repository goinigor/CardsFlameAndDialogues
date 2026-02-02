using CFD.Core;
using CFD.UI;
using UnityEngine;

namespace CFD
{
    /// <summary>
    /// Game starting point.
    /// This class is used to initialize core services and load the first scene.
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private SceneControllerService _sceneControllerService;
        [SerializeField] private LoadingScreen _loadingScreen;
        
        private void Awake()
        {
            ServiceLocator.Register<LoadingScreen>(_loadingScreen);
            ServiceLocator.Register<ISceneController>(_sceneControllerService);
            ServiceLocator.Register<API>(new API());
            
            _sceneControllerService.Initialize();
            
            DontDestroyOnLoad(_loadingScreen.gameObject);
            DontDestroyOnLoad(_sceneControllerService.gameObject);
        }

        private void Start()
        {
            _sceneControllerService.LoadScene(1);
        }
    }
}
