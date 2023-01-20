using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int MoveHash = Animator.StringToHash("Walking");
        private static readonly int AttackHash = Animator.StringToHash("AttackNormal");
        private static readonly int HitHash = Animator.StringToHash("Hit");
        private static readonly int DieHash = Animator.StringToHash("Die");

        private static readonly int IdleStateHash = Animator.StringToHash("Idle");
        private static readonly int IdleStateFullHash = Animator.StringToHash("Base Layer.Idle");
        private static readonly int AttackStateHash = Animator.StringToHash("Attack Normal");
        private static readonly int WalkingStateHash = Animator.StringToHash("Run");
        private static readonly int DeathStateHash = Animator.StringToHash("Die");

        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;

        public bool IsAttacking => State == AnimatorState.Attack;

        private void Update() => _animator.SetFloat(MoveHash, _characterController.velocity.magnitude, 0.1f, Time.deltaTime);

        public AnimatorState State { get; private set; }

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash) => StateExited?.Invoke(StateFor(stateHash));

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public void PlayHit() => _animator.SetTrigger(HitHash);
        public void PlayAttack() => _animator.SetTrigger(AttackHash);
        public void PlayDeath() => _animator.SetTrigger(DieHash);
        public void ResetToIdle() => _animator.Play(IdleStateHash, -1);

        private static AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == IdleStateHash)
            {
                state = AnimatorState.Idle;
            }
            else if (stateHash == AttackStateHash)
            {
                state = AnimatorState.Attack;
            }
            else if (stateHash == WalkingStateHash)
            {
                state = AnimatorState.Walking;
            }
            else if (stateHash == DeathStateHash)
            {
                state = AnimatorState.Died;
            }
            else
            {
                state = AnimatorState.Unknown;
            }

            return state;
        }
    }
}