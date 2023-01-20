﻿using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
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
        private Stats _heroStats;
        private IInputService _inputService;

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

        public void ReadFromProgress(PlayerProgress progress) =>
            _heroStats = progress.HeroStats;

        public void OnAttack() // called from animation events
        {
            int hitCount = Hit();

            PhysicsDebug.DrawDebug(StartPoint(), _heroStats.DamageRadius, 3f);

            for (var i = 0; i < hitCount; i++)
            {
                Collider hit = _hits[i];
                hit.transform.parent.GetComponent<IHealth>().TakeDamage(_heroStats.Damage);
            }
        }

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(StartPoint(), _heroStats.DamageRadius, _hits, _layerMask);

        private Vector3 StartPoint()
        {
            Transform transform1 = transform;

            Vector3 transformPosition = transform1.position + transform1.forward;
            transformPosition.y += _characterController.center.y / 2;
            return transformPosition;
        }
    }
}