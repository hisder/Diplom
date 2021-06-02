using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanElement
{
    public string Name { get; protected set; }

    public virtual void Pass()
    {
        Debug.Log("no realization");
    }
}
