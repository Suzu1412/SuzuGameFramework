using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Scene Management/Scene Database")]
public class SceneDatabaseSO : ScriptableObject
{
    [SerializeField] private List<SceneDataSO> _scenes;

    private Dictionary<SceneType, SceneDataSO> _lookup;

    public SceneDataSO GetScene(SceneType type)
    {
        if (_lookup == null)
        {
            _lookup = new Dictionary<SceneType, SceneDataSO>();
            foreach (var scene in _scenes)
                _lookup[scene.SceneType] = scene;
        }

        return _lookup[type];
    }
}
