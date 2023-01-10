using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine { get; }

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
        {
            var sceneLoader = new SceneLoader(coroutineRunner);

            StateMachine = new GameStateMachine(sceneLoader, curtain, AllServices.Container);
        }
    }
}