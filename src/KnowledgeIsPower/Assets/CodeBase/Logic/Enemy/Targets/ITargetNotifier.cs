using System;
using UnityEngine;

namespace CodeBase.Logic.Enemy.Targets
{
    public interface ITargetNotifier
    {
        public event Action<Transform> NewTarget;
        public event Action LostTarget;
    }
}