using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class GameLoopState : IState
    {
        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public class Factory : PlaceholderFactory<GameLoopState>
        {
        }
    }
}