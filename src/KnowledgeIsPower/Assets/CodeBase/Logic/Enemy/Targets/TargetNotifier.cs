using System;
using UnityEngine;

namespace CodeBase.Logic.Enemy.Targets
{
    [DisallowMultipleComponent]
    public abstract class TargetNotifier : MonoBehaviour
    {
        public event Action<Transform> NewTarget;
        public event Action LostTarget;

        protected void OnNewTarget(Transform target) => NewTarget?.Invoke(target);
        protected void OnLostTarget() => LostTarget?.Invoke();
    }
}