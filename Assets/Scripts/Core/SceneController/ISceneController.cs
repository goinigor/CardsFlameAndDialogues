using System;

namespace CFD.Core
{
    /// <summary>
    /// Interface for scene management operations
    /// </summary>
    public interface ISceneController : IService
    {
        void LoadScene(int buildIndex);
        void LoadSceneAsync(int buildIndex, Action onComplete = null);
        void LoadMainMenu();
    }
}
