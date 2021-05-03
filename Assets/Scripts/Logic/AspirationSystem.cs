using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspirationSystem
{
    public List<DustCollector> Collectors { get; private set; }
    public List<DustCollector> UniqueCollectors { get; private set; }


    public const int INDEX_ENTER = 0;
    public const int INDEX_EXIT = 1;


    public AspirationSystem(DustCollector enter, DustCollector exit)
    {
        Collectors = new List<DustCollector>();
        UniqueCollectors = new List<DustCollector>();

        Collectors.Add(enter);
        Collectors.Add(exit);
    }

    public void AddUniqueCollector(DustCollector newCollector)
    {
        Debug.Log(newCollector.Name);
        UniqueCollectors.Add(newCollector);
    }

    public void AddDustCollector(DustCollector newCollector)
    {
        Collectors.Add(newCollector);
    }


    public GasFlow GetCleanedFlow(GasFlow baseFlow)
    {
        DustCollector currentCollector;
        GasFlow cleanedGasFlow;

        currentCollector = Collectors[INDEX_ENTER];
        cleanedGasFlow = baseFlow.DeepCopy();


        if (CheckCompleteness())
        {
            while (currentCollector != Collectors[INDEX_EXIT])
            {
                currentCollector.Clean(cleanedGasFlow);
                currentCollector = currentCollector.ExitCollector;
            }
        }

        Debug.Log($"Система завершена - {CheckCompleteness()} дописать расчет");
        return cleanedGasFlow;
    }


    public bool CheckCompleteness()
    {
        int passedCollectorsCount = 0;
        DustCollector currentCollector;

        currentCollector = Collectors[INDEX_ENTER];

        while (passedCollectorsCount < Collectors.Count && currentCollector.ExitCollector != null)
        {
            currentCollector = currentCollector.ExitCollector;
            passedCollectorsCount++;
        }

        return currentCollector == Collectors[INDEX_EXIT];
    }
}
