using UnityEngine;

public interface IPausable
{
    void OnPaused();
    void OnResumed();

    GameObject GetGameObject();
}