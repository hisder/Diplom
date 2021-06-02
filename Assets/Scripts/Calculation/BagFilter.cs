using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagFilter : CleanElement
{
    private double _efficiency;

    public BagFilter(string name, double efficiency)
    {
        _efficiency = efficiency;
    }

    public override void Pass()
    {
        base.Pass();
    }
}
