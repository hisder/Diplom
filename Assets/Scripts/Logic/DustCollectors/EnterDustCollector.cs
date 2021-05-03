using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDustCollector : DustCollector
{
    public EnterDustCollector(string name, float[] efficiency) : base(name, efficiency) { }

    public override bool TryConnect(DustCollector connnectedCollector, ConnectionSide connectionSide)
    {
        if (connnectedCollector == this || connnectedCollector == null)
        {
            return false;
        }

        if (ExitCollector == null)
        {
            ExitCollector = connnectedCollector;
            connnectedCollector.EnterCollector = this;
            return true;
        }

        return false;
    }
}
