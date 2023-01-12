using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Win = Animator.StringToHash("Win");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Attack = Animator.StringToHash("Attack_1");

        private static readonly int IdleStateHash = Animator.StringToHash("idle");
        private static readonly int GetHitStateHash = Animator.StringToHash("GetHit");
        private static readonly int VictoryStateHash = Animator.StringToHash("victory");
        private static readonly int DieStateHash = Animator.StringToHash("die");
        private static readonly int MoveStateHash = Animator.StringToHash("Move");
        private static readonly int Attack1StateHash = Animator.StringToHash("attack01");

        private Animator _animator;

        private void Awake() =>
            _animator = GetComponent<Animator>();

        public AnimatorState State { get; private set; }

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            State = StateFor(stateHash);
            StateExited?.Invoke(State);
        }

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public void PlayHit() => _animator.SetTrigger(Hit);
        public void PlayDeath() => _animator.SetTrigger(Die);
        public void PlayWin() => _animator.SetTrigger(Win);
        public void PlayAttack() => _animator.SetTrigger(Attack);
        public void Move(float speed) => _animator.SetFloat(Speed, speed);
        public void StopMoving() => _animator.SetFloat(Speed, 0);

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == IdleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == Attack1StateHash)
                state = AnimatorState.Attack;
            else if (stateHash == MoveStateHash)
                state = AnimatorState.Walking;
            else if (stateHash == DieStateHash)
                state = AnimatorState.Died;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}