using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private Game _game;

        [Inject]
        public void Construct(Game game) =>
            _game = game;

        private void Awake() =>
            _game.StateMachine.Startup();
    }
}