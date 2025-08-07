using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    void Move();

    void CheckHits(List<Collider2D> resultsBuffer);

    GameObject GetGameObject();
}
