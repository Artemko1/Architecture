using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgressWriter> ProgressWriters { get; }
        GameObject HeroGameObject { get; }
        event Action HeroCreated;

        GameObject CreateHero(Vector3 initialPoint);
        GameObject CreateHud();
        void Cleanup();
    }
}