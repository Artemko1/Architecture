using UnityEngine;

namespace CodeBase.Logic.Enemy.Targets
{
    [RequireComponent(typeof(ITargetNotifier))]
    public abstract class HasTargetBehaviour : MonoBehaviour
    {
        private ITargetNotifier _targetNotifier;
        [field: SerializeField] protected Transform Target { get; private set; }

        protected virtual void Awake() =>
            _targetNotifier = GetComponent<ITargetNotifier>();

        private void OnEnable()
        {
            _targetNotifier.NewTarget += OnNewTarget;
            _targetNotifier.LostTarget += OnLostTarget;
        }

        private void OnDisable()
        {
            _targetNotifier.NewTarget -= OnNewTarget;
            _targetNotifier.LostTarget -= OnLostTarget;
        }

        protected bool HasTarget() => Target != null;

        protected virtual void OnNewTarget(Transform target) =>
            Target = target;

        protected virtual void OnLostTarget() =>
            Target = null;
    }
}