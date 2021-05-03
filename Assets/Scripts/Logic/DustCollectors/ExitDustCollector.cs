using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDustCollector : DustCollector
{
    public ExitDustCollector(string name, float[] efficiency) : base(name, efficiency) { }

    public override bool TryConnect(DustCollector connnectedCollector, ConnectionSide connectionSide)
    {
        if (connnectedCollector == this || connnectedCollector == null)
        {
            return false;
        }

        if (EnterCollector == null)
        {
            EnterCollector = connnectedCollector;
            connnectedCollector.ExitCollector = this;
            return true;
        }

        return false;
    }
}
