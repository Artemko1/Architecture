using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator)), RequireComponent(typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        private static int _layerMask;

        private readonly Collider[] _hits = new Collider[3];

        private CharacterController _characterController;
        private HeroAnimator _heroAnimator;
        private IInputService _inputService;
        private Stats _heroStats;

        private void Awake()
        {
            _heroAnimator = GetComponent<HeroAnimator>();
            _characterController = GetComponent<CharacterController>();

            _inputService = AllServices.Container.Single<IInputService>();

            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if (_inputService.IsAttackButtonUp() && !_heroAnimator.IsAttacking)
            {
                _heroAnimator.PlayAttack();
            }
        }

        public void OnAttack()
        {
            if (Hit() > 0)
            {
                PhysicsDebug.DrawDebug(StartPoint(), _heroStats.DamageRadius, 1.5f);
                // todo apply damage to enemy
            }
        }

        private int Hit()
        {
            return Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _heroStats.DamageRadius, _hits, _layerMask);
        }

        private Vector3 StartPoint()
        {
            Vector3 transformPosition = transform.position;
            return new Vector3(transformPosition.x, _characterController.center.y / 2, transformPosition.z);
        }

        public void ReadFromProgress(PlayerProgress progress) => 
            _heroStats = progress.HeroStats;
    }
}