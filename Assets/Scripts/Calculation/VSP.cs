using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSP : CleanElement
{
    private Func<double, double, double> _efficiencyHandler;

    public VSP(string name, Func<double, double, double> efficiencyHandler)
    {
        Name = Name;
        _efficiencyHandler = efficiencyHandler;
    }

    public override void Pass()
    {
        base.Pass();
    }
}
