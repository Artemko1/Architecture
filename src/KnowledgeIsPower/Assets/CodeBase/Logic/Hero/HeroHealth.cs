using System;
using CodeBase.Data;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgressReader, IHealth
    {
        private HeroAnimator _heroAnimator;
        private ISaveLoadService _saveLoadService;
        private State _state;

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

        public float Current
        {
            get => _state.CurrentHP;
            set
            {
                float clampedHP = Mathf.Clamp(value, 0, Max);
                if (clampedHP == _state.CurrentHP) return;

                _state.CurrentHP = clampedHP;
                HealthChanged?.Invoke();
            }
        }


        public float Max
        {
            get => _state.MaxHP;
            set => _state.MaxHP = value;
        }

        public void TakeDamage(float damage)
        {
            if (Current <= 0) return;
            Current -= damage;

            _heroAnimator.PlayHit();
        }

        public void ReadFromProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            HealthChanged?.Invoke();
        }

        private void WriteToProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHP = Current;
            progress.HeroState.MaxHP = Max;
        }
    }
}