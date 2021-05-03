using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfficiencyCalculator
{
    public static float CalculateEfficiency(GasFlow originalFlow, GasFlow cleanedFlow)
    {
        float result;
        result = 0;

        for (int i = 0; i < 5; i++)
        {
            if (originalFlow.Fullness[i] == 0)
            {
                continue;
            }

            Debug.Log($"(1f / 5f) заменить на gasSeparration {result}");
            result += (1 - cleanedFlow.Fullness[i] / originalFlow.Fullness[i]) * (1f / 5f) * 100f;
        }

        result = float.Parse(result.ToString("F" + 2));

        return result;
    }

    public static float CalculateEfficiency(DustCollector collector)
    {
        GasFlow original;
        GasFlow cleaned;

        original = new GasFlow(new float[5] { 1f, 1f, 1f, 1f, 1f });
        cleaned = original.DeepCopy();

        collector.Clean(cleaned);

        return EfficiencyCalculator.CalculateEfficiency(original, cleaned);
    }
}
