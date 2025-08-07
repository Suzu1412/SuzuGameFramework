using System.Collections.Generic;
using UnityEngine;

public interface IGameEventBinding
{
    List<GameObject> GetSubscribedGameObjects();
}
