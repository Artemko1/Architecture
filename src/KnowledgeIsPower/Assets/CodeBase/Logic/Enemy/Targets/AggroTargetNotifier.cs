using System.Collections;
using UnityEngine;

namespace CodeBase.Logic.Enemy.Targets
{
    public class AggroTargetNotifier : TargetNotifier
    {
        private const float Delay = 3f;

        [SerializeField] private TriggerObserver _triggerObserver;

        private Coroutine _aggroProlongCoroutine;
        private bool _hasTarget;

        private void OnEnable()
        {
            _triggerObserver.TriggerEnter += StartAggro;
            _triggerObserver.TriggerExit += StopAggro;
            _triggerObserver.Recalculate();
        }

        private void OnDisable()
        {
            _triggerObserver.TriggerEnter -= StartAggro;
            _triggerObserver.TriggerExit -= StopAggro;

            StopAggroInstant();
        }

        private void StartAggro(Collider other)
        {
            if (_hasTarget) return;

            _hasTarget = true;
            StopAggroProlongCoroutine();
            OnNewTarget(other.transform);
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
            OnLostTarget();
        }

        private void StopAggroInstant()
        {
            _hasTarget = false;
            StopAggroProlongCoroutine();
            OnLostTarget();
        }
    }
}