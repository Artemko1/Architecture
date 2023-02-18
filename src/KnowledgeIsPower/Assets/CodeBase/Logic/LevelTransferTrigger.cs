using CodeBase.Infrastructure.States;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
    public class LevelTransferTrigger : MonoBehaviour
    {
        public string TransferTo;
        private GameStateMachine _stateMachine;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine) =>
            _stateMachine = gameStateMachine;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<CharacterController>()) return;
            _stateMachine.Enter<LoadLevelState, string>(TransferTo);
            gameObject.SetActive(false);
        }
    }
}