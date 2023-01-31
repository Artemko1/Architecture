using System;
using UnityEngine;

namespace CodeBase.Logic
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour
    {
        private Collider _collider;

        private void OnTriggerEnter(Collider other) => TriggerEnter?.Invoke(other);

        private void OnTriggerExit(Collider other) => TriggerExit?.Invoke(other);

        public event Action<Collider> TriggerEnter;
        public event Action<Collider> TriggerExit;

        public void Recalculate()
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider>();
            }

            // ReSharper disable Unity.InefficientPropertyAccess
            _collider.enabled = false;
            _collider.enabled = true;
            // ReSharper restore Unity.InefficientPropertyAccess
        }
    }
}