using CodeBase.Infrastructure.States;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        [Inject]
        public Game(GameStateMachine gameStateMachine)
        {
            StateMachine = gameStateMachine;
        }

        public GameStateMachine StateMachine { get; }
    }
}