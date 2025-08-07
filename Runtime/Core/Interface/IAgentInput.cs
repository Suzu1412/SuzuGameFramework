using System;
using UnityEngine;

public interface IAgentInput
{
    event Action<Vector2> OnMovement;
    event Action OnAttackPressed;
    event Action OnAttackReleased;
    event Action OnAttackHeld;
}