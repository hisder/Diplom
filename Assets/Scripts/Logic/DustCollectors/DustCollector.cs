using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DustCollector
{
    [SerializeField] private string _name;
    public string Name { get => _name; protected set => _name = value; }

    [SerializeField] private float[] _efficiency;
    public float[] Efficiency { get => _efficiency; protected set => _efficiency = value; }

    [SerializeField] private DustCollector _enterCollector;
    public DustCollector EnterCollector { get => _enterCollector; set => _enterCollector = value; }

    [SerializeField] private DustCollector _exitCollector;
    public DustCollector ExitCollector { get => _exitCollector; set => _exitCollector = value; }

    public int Throughput { get; private set; }

    public void SetThroughput(int value)
    {
        Throughput = value;
    }

    public DustCollector(string name, float[] efficiency)
    {
        Name = name;
        Efficiency = efficiency;
    }

    public void Clean(GasFlow gasFlow)
    {
        for (int i = 0; i < 5; i++)
        {
            gasFlow.Fullness[i] -= gasFlow.Fullness[i] * Efficiency[i];
        }
    }

    public virtual bool TryConnect(DustCollector connectedCollector, ConnectionSide connectionSide)
    {
        Debug.Log($"{this} Connection");
        if (connectedCollector == this ||
            connectedCollector == null)
        {
            return false;
        }

        switch (connectionSide)
        {
            case ConnectionSide.Left:
                if (EnterCollector == null)
                {
                    EnterCollector = connectedCollector;
                    connectedCollector.ExitCollector = this;
                    return true;
                }
                break;

            case ConnectionSide.Right:
                if (ExitCollector == null)
                {
                    ExitCollector = connectedCollector;
                    connectedCollector.EnterCollector = this;
                    return true;
                }
                break;
        }

        return false;
    }

    public void Disconnect(DustCollector connectedCollector)
    {
        if (ExitCollector == connectedCollector)
        {
            ExitCollector = null;
            connectedCollector.EnterCollector = null;
        }
        else if (EnterCollector == connectedCollector)
        {
            EnterCollector = null;
            connectedCollector.ExitCollector = null;
        }
    }
}
