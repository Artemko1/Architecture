using System.Collections;
using UnityEngine;

namespace CodeBase.Logic.Enemy
{
    [RequireComponent(typeof(HasTargetBehaviour))]
    public class AggroTargetAssigner : MonoBehaviour
    {
        private const float Delay = 3f;

        [SerializeField] private TriggerObserver _triggerObserver;

        private Coroutine _aggroProlongCoroutine;
        private bool _hasTarget;

        private HasTargetBehaviour[] _targetsComponents;

        private void Awake()
        {
            _targetsComponents = GetComponents<HasTargetBehaviour>();
            ResetTargets();
        }

        private void OnEnable()
        {
            _triggerObserver.TriggerEnter += StartAggro;
            _triggerObserver.TriggerExit += StopAggro;
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEnter -= StartAggro;
            _triggerObserver.TriggerExit -= StopAggro;
        }

        private void StartAggro(Collider other)
        {
            if (_hasTarget) return;

            _hasTarget = true;
            StopAggroProlongCoroutine();
            SetTargets(other.transform);
        }

        private void StopAggro(Collider other)
        {
            if (!_hasTarget) return;

            _hasTarget = false;
            _aggroProlongCoroutine = StartCoroutine(ResetTargetsAfterDelay());
        }

        private void StopAggroProlongCoroutine()
        {
            if (_aggroProlongCoroutine == null) return;

            StopCoroutine(_aggroProlongCoroutine);
            _aggroProlongCoroutine = null;
        }

        private IEnumerator ResetTargetsAfterDelay()
        {
            yield return new WaitForSeconds(Delay);
            ResetTargets();
        }

        private void SetTargets(Transform otherTransform)
        {
            foreach (HasTargetBehaviour hasTarget in _targetsComponents)
            {
                hasTarget.SetTarget(otherTransform);
            }
        }

        private void ResetTargets()
        {
            foreach (HasTargetBehaviour hasTarget in _targetsComponents)
            {
                hasTarget.ResetTarget();
            }
        }
    }
}