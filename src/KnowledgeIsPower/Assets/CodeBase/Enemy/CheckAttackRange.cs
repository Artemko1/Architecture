using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Attack))]
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _triggerObserver;
        private Attack _attack;

        private void Awake()
        {
            _attack = GetComponent<Attack>();

            _triggerObserver.TriggerEnter += OnTriggerEnter;
            _triggerObserver.TriggerExit += OnTriggerExit;

            _attack.DisableAttack();
        }

        private void OnTriggerEnter(Collider obj)
        {
            _attack.EnableAttack();
        }

        private void OnTriggerExit(Collider obj)
        {
            _attack.DisableAttack();
        }
    }
}