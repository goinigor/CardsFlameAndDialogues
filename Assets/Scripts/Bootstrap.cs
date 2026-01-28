using CFD.Core;
using UnityEngine;

namespace CFD
{
    /// <summary>
    /// This class is used to initialize core services and load the first scene.
    /// </summary>
    public class Bootstrap : MonoBehaviour
    {
        //TODO add loading screen
        [SerializeField] private SceneControllerService _sceneControllerService;
        
        private void Awake()
        {
            ServiceLocator.Register<ISceneController>(_sceneControllerService);
            _sceneControllerService.Initialize();
            DontDestroyOnLoad(_sceneControllerService.gameObject);
            
        }

        private void Start()
        {
            _sceneControllerService.LoadScene(1);
        }
    }
}
