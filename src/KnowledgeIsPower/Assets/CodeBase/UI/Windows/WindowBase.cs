﻿using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public Button CloseButton;
        private IPersistentProgressService _progressService;

        protected PlayerProgress Progress => _progressService.Progress;

        protected virtual void Awake() => CloseButton.onClick.AddListener(() => Destroy(gameObject));

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() =>
            Cleanup();

        public void Construct(IPersistentProgressService progressService) => _progressService = progressService;

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