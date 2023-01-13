using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Follow))]
    public class Aggro : MonoBehaviour
    {
        private const float Delay = 3f;

        [SerializeField] private TriggerObserver _triggerObserver;

        private Coroutine _aggroCoroutine;

        private Follow _follow;
        private bool _hasTarget;

        private void Awake()
        {
            _follow = GetComponent<Follow>();
            DisableFollow();
        }

        private void Start()
        {
            _triggerObserver.TriggerEnter += OnTriggerEnter;
            _triggerObserver.TriggerExit += OnTriggerExit;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasTarget) return;

            _hasTarget = true;
            StopAggroCoroutine();
            EnableFollow();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_hasTarget) return;

            _hasTarget = false;
            _aggroCoroutine = StartCoroutine(DisableFollowAfterDelay());
        }

        private void StopAggroCoroutine()
        {
            if (_aggroCoroutine == null) return;

            StopCoroutine(_aggroCoroutine);
            _aggroCoroutine = null;
        }

        private IEnumerator DisableFollowAfterDelay()
        {
            yield return new WaitForSeconds(Delay);
            DisableFollow();
        }

        private void EnableFollow() => _follow.enabled = true;
        private void DisableFollow() => _follow.enabled = false;
    }
}