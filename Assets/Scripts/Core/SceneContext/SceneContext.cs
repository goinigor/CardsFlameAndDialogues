using System.Collections.Generic;
using CFD.UI.Menu;
using UnityEngine;

namespace CFD.Core
{
    /// <summary>
    /// Similar to the Bootstrap class, but for the current scene context. Initialize and resolve dependencies
    /// </summary>
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private List<AbstractMonoInstaller> _installers;
        
        private MainMenuPresenter _mainMenuPresenter;

        private void Awake()
        {
            foreach (var installer in _installers)
            {
                installer.InstallBindings();
            }
        }

        private void Start()
        {
            foreach (var installer in _installers)
            {
                installer.Initialize();
            }
        }

        private void OnDestroy()
        {
            foreach (var installer in _installers)
            {
                installer.Dispose();
            }
        }
    }
}