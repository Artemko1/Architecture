using UnityEngine;

namespace CodeBase.Logic.Enemy
{
    public abstract class HasTargetBehaviour : MonoBehaviour
    {
        [SerializeField] protected Transform Target;

        public bool HasTarget => Target != null;

        public void SetTarget(Transform target) => Target = target;

        // enabled = true;
        public virtual void ResetTarget() => Target = null;
        // enabled = false;
    }
}