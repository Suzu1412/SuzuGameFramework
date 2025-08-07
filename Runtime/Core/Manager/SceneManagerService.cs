using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneManagerService : PersistentSingleton<SceneManagerService>
{
    [SerializeField] private List<SceneDataSO> _persistentScenes;
    [SerializeField] private SceneDatabaseSO _sceneDatabase;
    [SerializeField] private SceneGameEventBinding _sceneBinding;
    [SerializeField] private FloatVariableSO _progress;

    private Scene _previousScene;
    private Scene _currentScene;

    [SerializeField] private VoidGameEvent _onSceneLoadingStarted;
    [SerializeField] private VoidGameEvent _onSceneLoadingFinished;

    private void Start()
    {
        _progress.Initialize(0f, 0f, 1f);
    }

    private void OnEnable()
    {
        _sceneBinding.Bind(LoadScene, this);
    }

    private void OnDisable()
    {
        _sceneBinding.Unbind(LoadScene, this);
    }

    public void LoadScene(SceneDataSO targetScene)
    {
        if (targetScene == null) return;

        StartCoroutine(LoadSceneFlow(targetScene));
    }

    private IEnumerator LoadSceneFlow(SceneDataSO targetScene)
    {
        _onSceneLoadingStarted.RaiseEvent(this);

        var operations = new List<AsyncOperation>();

        // Load missing persistent scenes
        foreach (var persistent in _persistentScenes)
        {
            if (!SceneManager.GetSceneByName(persistent.SceneName).isLoaded)
            {
                var loadOp = SceneManager.LoadSceneAsync(persistent.SceneName, LoadSceneMode.Additive);
                loadOp.allowSceneActivation = false;
                operations.Add(loadOp);
            }
        }

        // Unload all non-persistent scenes (excluding target)
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded &&
                !_persistentScenes.Any(p => p.SceneName == scene.name) &&
                scene.name != targetScene.SceneName)
            {
                var unloadOp = SceneManager.UnloadSceneAsync(scene);
                operations.Add(unloadOp);
            }
        }

        // Load target scene only if not loaded
        if (!SceneManager.GetSceneByName(targetScene.SceneName).isLoaded)
        {
            var loadOp = SceneManager.LoadSceneAsync(targetScene.SceneName, LoadSceneMode.Additive);
            loadOp.allowSceneActivation = false;
            operations.Add(loadOp);
        }
        else
        {
            Debug.LogWarning($"Scene '{targetScene.SceneName}' is already loaded. Skipping load.");
        }

        // Track loading progress
        while (!AllDone(operations))
        {
            float totalProgress = 0f;
            foreach (var op in operations)
            {
                if (op != null && !op.isDone)
                {
                    totalProgress += Mathf.Clamp01(op.progress / 0.9f);
                }
            }

            float average = operations.Count > 0 ? totalProgress / operations.Count : 1f;
            _progress?.SetValue(average);
            yield return null;
        }

        // Allow activation for all loading ops
        foreach (var op in operations)
        {
            if (op != null && !op.isDone && !op.allowSceneActivation)
            {
                op.allowSceneActivation = true;
            }
        }

        // Wait for all to complete
        foreach (var op in operations)
        {
            while (op != null && !op.isDone)
            {
                yield return null;
            }
        }

        // Set the newly loaded scene as active (only if it was loaded now)
        Scene newlyLoaded = SceneManager.GetSceneByName(targetScene.SceneName);
        if (newlyLoaded.IsValid() && newlyLoaded.isLoaded)
        {
            _currentScene = newlyLoaded;
            SceneManager.SetActiveScene(_currentScene);
        }

        _progress?.SetValue(1f);
        _onSceneLoadingFinished.RaiseEvent(this);
    }

    private bool AllDone(List<AsyncOperation> operations)
    {
        foreach (var op in operations)
        {
            if (op.allowSceneActivation == false && op.progress < 0.9f)
                return false;

            if (op.allowSceneActivation && !op.isDone)
                return false;
        }

        return true;
    }
}
