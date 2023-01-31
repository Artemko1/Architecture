using System.Linq;
using UnityEngine;

namespace CodeBase.Logic.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : HasTargetBehaviour
    {
        public float Cleavage = 0.8f;
        public float EffectiveDistance = 0.75f;
        public float Damage = 5;

        [SerializeField] private float _attackCooldown = 2.5f;
        [SerializeField] private TriggerObserver _rangeObserver;

        private readonly Collider[] _hits = new Collider[1];

        private float _attackCooldownRemaining;

        private EnemyAnimator _enemyAnimator;

        private bool _isAttacking;
        private bool _isInRange;
        private int _layerMask;

        private void Awake()
        {
            _enemyAnimator = GetComponent<EnemyAnimator>();
            _layerMask = 1 << LayerMask.NameToLayer("Player");

            _rangeObserver.TriggerEnter += _ => _isInRange = true;
            _rangeObserver.TriggerExit += _ => _isInRange = false;
        }

        private void Update()
        {
            UpdateCooldown();

            if (CanAttack())
            {
                StartAttack();
            }
        }

        private void OnAttack() // called from animation events
        {
            if (!Hit(out Collider hit)) return;

            PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1.5f);
            hit.GetComponent<IHealth>().TakeDamage(Damage);
        }

        private void OnAttackEnd() // called from animation events
        {
            _attackCooldownRemaining = _attackCooldown;
            _isAttacking = false;
        }

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitsCount > 0;
        }

        private Vector3 StartPoint()
        {
            Transform transform1 = transform;

            Vector3 startPoint = transform1.position;
            startPoint.y += 0.5f;
            startPoint += transform1.forward * EffectiveDistance;
            return startPoint;
        }

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
            {
                _attackCooldownRemaining -= Time.deltaTime;
            }
        }

        private bool CanAttack() =>
            !_isAttacking && CooldownIsUp() && _isInRange && HasTarget;

        private bool CooldownIsUp() =>
            _attackCooldownRemaining <= 0;

        private void StartAttack()
        {
            transform.LookAt(Target);
            _enemyAnimator.PlayAttack();
            _isAttacking = true;
        }
    }
}