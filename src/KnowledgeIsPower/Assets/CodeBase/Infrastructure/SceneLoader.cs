using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        private int _remainingLoadings;
        private bool _sceneInitialized;

        [Inject]
        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string name, Action onLoaded = null)
        {
            Assert.IsFalse(_sceneInitialized);

            string activeScene = SceneManager.GetActiveScene().name;
            if (activeScene == name)
            {
                Debug.Log($"Scene {name} is already loaded");
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperationHandle<SceneInstance> waitNextScene = Addressables.LoadSceneAsync(name);

            yield return new WaitUntil(() => waitNextScene.IsDone);
            yield return new WaitUntil(() => _sceneInitialized);

            _sceneInitialized = false;

            onLoaded?.Invoke();
        }

        public void RegisterLoading() =>
            _remainingLoadings++;

        public void UnregisterLoading()
        {
            Assert.IsTrue(_remainingLoadings > 0);

            _remainingLoadings--;
            if (_remainingLoadings == 0)
            {
                _sceneInitialized = true;
            }
        }
    }
}