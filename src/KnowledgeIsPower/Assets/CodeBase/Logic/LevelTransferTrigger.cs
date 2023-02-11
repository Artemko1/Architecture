using CodeBase.Infrastructure.States;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LevelTransferTrigger : MonoBehaviour
    {
        public string TransferTo;
        private IGameStateMachine _stateMachine;

        private void Awake() =>
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<CharacterController>()) return;
            _stateMachine.Enter<LoadLevelState, string>(TransferTo);
            gameObject.SetActive(false);
        }
    }
}