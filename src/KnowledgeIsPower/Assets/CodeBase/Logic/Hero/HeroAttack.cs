using CodeBase.Logic.Enemy;
using CodeBase.Services.Input;
using CodeBase.StaticData.ForComponents;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(HeroAnimator)), RequireComponent(typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour
    {
        private static int _layerMask;

        private readonly Collider[] _hits = new Collider[3];
        private AttackData _attackData;

        private CharacterController _characterController;
        private HeroAnimator _heroAnimator;
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService) =>
            _inputService = inputService;

        public void Construct(AttackData attackData) =>
            _attackData = attackData;

        private void Awake()
        {
            _heroAnimator = GetComponent<HeroAnimator>();
            _characterController = GetComponent<CharacterController>();

            _layerMask = 1 << LayerMask.NameToLayer(Constants.Layers.Hittable);
        }

        private void Update()
        {
            if (_inputService.IsAttackButtonUp() && !_heroAnimator.IsAttacking)
            {
                _heroAnimator.PlayAttack();
            }
        }

        public void OnAttack() // called from animation events
        {
            int hitCount = Hit();

            PhysicsDebug.DrawDebug(StartPoint(), _attackData.Radius, 3f);

            for (var i = 0; i < hitCount; i++)
            {
                Collider hit = _hits[i];
                hit.transform.parent.GetComponent<IHealth>().TakeDamage(_attackData.Damage);
            }
        }

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(StartPoint(), _attackData.Radius, _hits, _layerMask);

        private Vector3 StartPoint()
        {
            Transform transform1 = transform;

            Vector3 transformPosition = transform1.position + transform1.forward;
            transformPosition.y += _characterController.center.y / 2;
            return transformPosition;
        }
    }
}