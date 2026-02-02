using System;
using UnityEngine;

namespace CFD.Core
{
    /// <summary>
    /// Base class for MonoInstallers. Used for scene context dependency injection
    /// </summary>
    public abstract class AbstractMonoInstaller : MonoBehaviour, IDisposable
    {
        public abstract void InstallBindings();
        public abstract void Initialize();
        public abstract void Dispose();
    }
}