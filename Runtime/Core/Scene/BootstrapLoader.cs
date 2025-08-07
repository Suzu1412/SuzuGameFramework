using Eflatun.SceneReference;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private SceneReference _managersScene;
    [SerializeField] private SceneDataSO _initialSceneSO;
    [SerializeField] private SceneGameEvent _sceneEvent;

    private void Start()
    {
        StartCoroutine(BootstrapFlow());
    }

    private IEnumerator BootstrapFlow()
    {
        // Load the persistent Managers scene
        var managersLoad = SceneManager.LoadSceneAsync(_managersScene.Name, LoadSceneMode.Additive);
        yield return managersLoad;

        // Wait one frame to ensure managers have initialized
        yield return null;

        // Optionally, wait for SceneManagerService to be ready
        //yield return WaitForSceneManagerService();

        // Start loading the first gameplay or menu scene
        _sceneEvent.RaiseEvent(_initialSceneSO, this);
        //SceneManagerService.Instance.LoadScene(_initialSceneSO);
    }

    //private IEnumerator WaitForSceneManagerService()
    //{
    //    while (SceneManagerService.Instance == null)
    //        yield return null;
    //}
}
