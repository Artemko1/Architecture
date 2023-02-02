using System;
using CodeBase.Data;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData.ForComponents;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgressReader, IHealth
    {
        [SerializeField] private float _current = 5;

        private HealthData _healthData;

        private HeroAnimator _heroAnimator;
        private ISaveLoadService _saveLoadService;

        private void Awake()
        {
            _heroAnimator = GetComponent<HeroAnimator>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void Start() =>
            _saveLoadService.OnSave += WriteToProgress;

        private void OnDestroy() =>
            _saveLoadService.OnSave -= WriteToProgress;

        public event Action HealthChanged;
        public event Action Died;

        public void Construct(HealthData healthData) =>
            _healthData = healthData;

        public float Current => _current;
        public float Max => _healthData.MaxHp;

        public void TakeDamage(float amount)
        {
            if (_current <= 0 || amount <= 0) return;

            _current = ClampCurrentHp(_current - amount);

            HealthChanged?.Invoke();

            if (_current <= 0)
            {
                _heroAnimator.PlayDeath();
                Died?.Invoke();
            }
            else
            {
                _heroAnimator.PlayHit();
            }
        }

        public void ReadFromProgress(PlayerProgress progress)
        {
            _current = ClampCurrentHp(progress.PlayerState.CurrentHP);
            HealthChanged?.Invoke();
        }

        private void WriteToProgress(PlayerProgress progress) =>
            progress.PlayerState.CurrentHP = Current;

        private float ClampCurrentHp(float newCurrent) =>
            Mathf.Clamp(newCurrent, 0, Max);
    }
}