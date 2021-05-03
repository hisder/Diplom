using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GasFlow
{
    [SerializeField] private float[] _fullness;
    public float[] Fullness { get => _fullness; private set => _fullness = value; }

    public GasFlow() { }

    public GasFlow(float[] fullness)
    {
        Fullness = fullness;
    }

    public GasFlow DeepCopy()
    {
        GasFlow copy = (GasFlow)this.MemberwiseClone();

        copy.Fullness = (float[])this.Fullness.Clone();
        return copy;
    }

    public new string ToString()
    {
        return $"Поток: {Fullness[0]} {Fullness[1]} {Fullness[2]} {Fullness[3]} {Fullness[4]}";
    }
}
