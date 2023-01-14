using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        private const float Cleavage = 1;
        private const float EffectiveDistance = 0.5f;

        [SerializeField] private float _attackCooldown = 2f;
        [SerializeField] private bool _logCanAttack;

        private readonly Collider[] _hits = new Collider[1];

        private float _attackCooldownRemaining;
        private bool _attackIsActive;

        private EnemyAnimator _enemyAnimator;

        private IGameFactory _gameFactory;
        private Transform _heroTransform;
        private bool _isAttacking;
        private int _layerMask;

        private void Awake()
        {
            _enemyAnimator = GetComponent<EnemyAnimator>();

            _layerMask = 1 << LayerMask.NameToLayer("Player");

            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _gameFactory.HeroCreated += InitializeHeroTransform;
        }

        private void Update()
        {
            UpdateCooldown();

            if (CanAttack())
            {
                StartAttack();
            }
        }

        public void EnableAttack() =>
            _attackIsActive = true;

        public void DisableAttack() =>
            _attackIsActive = false;

        private void OnAttack() // called from animation events
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1.5f);
            }
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

        private bool CanAttack()
        {
            bool canAttack = !_isAttacking && CooldownIsUp() && _attackIsActive;
            if (_logCanAttack)
            {
                Debug.Log(canAttack);
            }

            return canAttack;
        }

        private bool CooldownIsUp() =>
            _attackCooldownRemaining <= 0;

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            _enemyAnimator.PlayAttack();
            _isAttacking = true;
        }

        private void InitializeHeroTransform() =>
            _heroTransform = _gameFactory.HeroGameObject.transform;
    }
}