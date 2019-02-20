using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum damageTypes
{
    falling,
    bullet,
    cannon,
    bomb
}

public interface IBreakable
{
    void Break(damageTypes damageType);
}

public interface IMoveable
{
    bool MoveTo(Vector3 newPosition);
    bool Rotate(Vector3 rotation);
    Vector2 GetDimensions();
    void SnapIntoPlace();
    void ReleaseGrab();
    bool CanMoveDirection(Vector3 direction);
}

public interface ITorchable
{
    void Torch();
}