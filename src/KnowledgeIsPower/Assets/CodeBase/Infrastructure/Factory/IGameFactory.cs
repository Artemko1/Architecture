﻿using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory
    {
        GameObject CreateHero(GameObject initialPoint);
        void CreateHud();
    }
}