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
        private Stats _heroStats;
        private ISaveLoadService _saveLoadService;
        private PlayerState _state;

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
            get => _heroStats.MaxHP;
            set => throw new NotImplementedException();
        }

        public void TakeDamage(float damage)
        {
            if (Current <= 0) return;
            Current -= damage;

            _heroAnimator.PlayHit();
        }

        public void ReadFromProgress(PlayerProgress progress)
        {
            _state = progress.PlayerState;
            HealthChanged?.Invoke();
        }

        public void Construct(Stats heroStats) =>
            _heroStats = heroStats;

        private void WriteToProgress(PlayerProgress progress) =>
            progress.PlayerState.CurrentHP = Current;
    }
}