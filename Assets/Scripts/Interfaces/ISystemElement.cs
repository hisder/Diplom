using System;
using UnityEngine;

public interface ISystemElement
{
    public void Move(Vector3 targetPosition);
    public void Destroy();
}
