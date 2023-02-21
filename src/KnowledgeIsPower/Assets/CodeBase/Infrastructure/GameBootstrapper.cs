using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : IInitializable
    {
        private readonly Game _game;

        [Inject]
        public GameBootstrapper(Game game)
        {
            _game = game;
        }

        public void Initialize() =>
            _game.StateMachine.Startup();
    }
}