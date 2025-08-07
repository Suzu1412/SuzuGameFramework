using UnityEngine;

public interface IAgent
{
    public Vector2 FacingDirection { get; }
    public Vector2 AimDirection{ get; }
}
