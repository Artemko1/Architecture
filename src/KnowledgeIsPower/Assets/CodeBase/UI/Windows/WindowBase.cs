using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public Button CloseButton;
        private PersistentProgressService _progressService;

        [Inject]
        public void Construct(PersistentProgressService progressService) =>
            _progressService = progressService;

        protected PlayerProgress Progress =>
            _progressService.Progress;

        protected virtual void Awake() => CloseButton.onClick.AddListener(() => Destroy(gameObject));

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() =>
            Cleanup();

        protected virtual void Initialize()
        {
        }

        protected virtual void SubscribeUpdates()
        {
        }

        protected virtual void Cleanup()
        {
        }
    }
}