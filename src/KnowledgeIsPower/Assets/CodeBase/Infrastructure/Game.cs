using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
        {
            var sceneLoader = new SceneLoader(coroutineRunner);

            StateMachine = new GameStateMachine(sceneLoader, curtain, AllServices.Container);
        }

        public GameStateMachine StateMachine { get; }
    }
}