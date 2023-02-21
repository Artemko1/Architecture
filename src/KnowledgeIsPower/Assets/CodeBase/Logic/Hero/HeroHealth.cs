using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData.ForComponents;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, IHealth
    {
        private HealthData _healthData;

        private HeroAnimator _heroAnimator;
        private PlayerProgress _progress;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(ISaveLoadService saveLoadService, PersistentProgressService progressService, HealthData healthData)
        {
            _saveLoadService = saveLoadService;
            _progress = progressService.Progress;
            _healthData = healthData;
        }

        private void Awake() =>
            _heroAnimator = GetComponent<HeroAnimator>();

        private void Start()
        {
            _saveLoadService.OnSave += WriteToProgress;

            Current = ClampCurrentHp(Current);
            HealthChanged?.Invoke();
        }

        private void OnDestroy() =>
            _saveLoadService.OnSave -= WriteToProgress;

        public event Action HealthChanged;
        public event Action Died;


        public float Current
        {
            get => _progress.PlayerState.CurrentHP;
            private set => _progress.PlayerState.CurrentHP = value;
        }

        public float Max => _healthData.MaxHp;

        public void TakeDamage(float amount)
        {
            if (Current <= 0 || amount <= 0) return;

            Current = ClampCurrentHp(Current - amount);

            HealthChanged?.Invoke();

            if (Current <= 0)
            {
                _heroAnimator.PlayDeath();
                Died?.Invoke();
            }
            else
            {
                _heroAnimator.PlayHit();
            }
        }

        public void Construct(HealthData healthData) =>
            throw new NotImplementedException();

        private void WriteToProgress(PlayerProgress progress) =>
            progress.PlayerState.CurrentHP = Current;

        private float ClampCurrentHp(float newCurrent) =>
            Mathf.Clamp(newCurrent, 0, Max);
    }
}