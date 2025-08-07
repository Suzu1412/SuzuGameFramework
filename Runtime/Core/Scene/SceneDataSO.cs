using Eflatun.SceneReference;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Scene Management/Scene Data")]
public class SceneDataSO : ScriptableObject
{
    [SerializeField] private SceneReference _sceneReference;
    [SerializeField] private SceneType _sceneType;

    public string SceneName => _sceneReference.Name;
    public SceneReference SceneReference => _sceneReference;
    public SceneType SceneType => _sceneType;
}
