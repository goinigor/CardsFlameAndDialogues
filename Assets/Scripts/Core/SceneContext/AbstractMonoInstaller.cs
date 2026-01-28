using System;
using UnityEngine;

namespace CFD.Core
{
    public abstract class AbstractMonoInstaller : MonoBehaviour, IDisposable
    {
        public abstract void InstallBindings();
        public abstract void Initialize();
        public abstract void Dispose();
    }
}