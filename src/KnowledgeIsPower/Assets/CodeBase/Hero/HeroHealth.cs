using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgressReader, ISavedProgressWriter, IHealth
    {
        private HeroAnimator _heroAnimator;
        private State _state;

        private void Awake() =>
            _heroAnimator = GetComponent<HeroAnimator>();

        public event Action HealthChanged;

        public float Current
        {
            get => _state.CurrentHP;
            private set
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
            private set => _state.MaxHP = value;
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

        public void WriteToProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHP = Current;
            progress.HeroState.MaxHP = Max;
        }
    }
}