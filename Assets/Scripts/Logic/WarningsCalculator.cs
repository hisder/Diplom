using UnityEngine;

public class WarningsCalculator
{
    public int СalculateWarningsNumber(AspirationSystem systemToCheck)
    {
        DustCollector currentCollector;

        GasFlow baseFlow;
        GasFlow cleanedGasFlow;
        GasFlow localBaseGasFlow;

        currentCollector = systemToCheck.Collectors[AspirationSystem.INDEX_ENTER];

        baseFlow = new GasFlow(new float[] { 1f, 1f, 1f, 1f, 1f });
        cleanedGasFlow = baseFlow.DeepCopy();

        while (currentCollector != systemToCheck.Collectors[AspirationSystem.INDEX_EXIT])
        {
            if (currentCollector == systemToCheck.Collectors[AspirationSystem.INDEX_EXIT])
            {
                break;
            }

            localBaseGasFlow = cleanedGasFlow.DeepCopy();

            DetermineRealEffectiveness(currentCollector, cleanedGasFlow);

            currentCollector.Clean(cleanedGasFlow);




            //if (EfficiencyCalculator.CalculateEfficiency(localBaseGasFlow, cleanedGasFlow) == 0)
            //{
            //    Debug.Log(currentCollector.Name);
            //}

            currentCollector = currentCollector.ExitCollector;
        }



        return 1;
    }

    private float DetermineRealEffectiveness(DustCollector collector, GasFlow baseFlow)
    {
        GasFlow cleanFlow;
        GasFlow fulllFlow;

        float fullEfficiency;
        float curentEfficiency;


        fulllFlow = new GasFlow(new float[] { 1f, 1f, 1f, 1f, 1f });
        cleanFlow = fulllFlow.DeepCopy();
        collector.Clean(cleanFlow);

        fullEfficiency = EfficiencyCalculator.CalculateEfficiency(fulllFlow, cleanFlow);

        fulllFlow = baseFlow.DeepCopy();
        cleanFlow = baseFlow.DeepCopy();
        collector.Clean(cleanFlow);

        curentEfficiency = EfficiencyCalculator.CalculateEfficiency(fulllFlow, cleanFlow);

        Debug.Log($"{fullEfficiency} {curentEfficiency} эффективность {collector.Name}");

        return fullEfficiency - curentEfficiency;
    }
}