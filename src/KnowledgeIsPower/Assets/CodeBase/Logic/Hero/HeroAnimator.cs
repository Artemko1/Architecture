using System;
using CodeBase.Logic.Animator;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    public class HeroAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int MoveHash = UnityEngine.Animator.StringToHash("Walking");
        private static readonly int AttackHash = UnityEngine.Animator.StringToHash("AttackNormal");
        private static readonly int HitHash = UnityEngine.Animator.StringToHash("Hit");
        private static readonly int DieHash = UnityEngine.Animator.StringToHash("Die");

        private static readonly int IdleStateHash = UnityEngine.Animator.StringToHash("Idle");
        private static readonly int IdleStateFullHash = UnityEngine.Animator.StringToHash("Base Layer.Idle");
        private static readonly int AttackStateHash = UnityEngine.Animator.StringToHash("Attack Normal");
        private static readonly int WalkingStateHash = UnityEngine.Animator.StringToHash("Run");
        private static readonly int DeathStateHash = UnityEngine.Animator.StringToHash("Die");

        [SerializeField] private UnityEngine.Animator _animator;
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