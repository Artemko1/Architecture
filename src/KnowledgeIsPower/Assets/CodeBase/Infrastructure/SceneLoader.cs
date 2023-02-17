﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string name, Action onLoaded = null)
        {
            string activeScene = SceneManager.GetActiveScene().name;
            Debug.Log($"Loading from {activeScene} to {name}...");
            if (activeScene == name)
            {
                Debug.Log($"Scene {name} is already loaded");
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name);

            yield return new WaitUntil(() => waitNextScene.isDone);

            onLoaded?.Invoke();
        }
    }
}