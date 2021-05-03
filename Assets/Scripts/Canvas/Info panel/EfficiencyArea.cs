using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EfficiencyArea : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _efficiencyTextArea;
    [SerializeField] private AspirationSystemView _systemView;

    private WarningsCalculator _warningsCalculator;


    private void Awake()
    {
        _warningsCalculator = new WarningsCalculator();

        _systemView.OnSystemClear += DisplatEfficiency;
        _systemView.OnSystemClear += DisplayWarningsCount;

        _systemView.OnSystemDoesntComplete += () => { _efficiencyTextArea.text = "NAN"; };
    }


    private void DisplatEfficiency(float efficiency)
    {
        _efficiencyTextArea.text = $"{efficiency.ToString()}%";
    }

    private void DisplayWarningsCount(float efficiency)
    {
        _warningsCalculator.СalculateWarningsNumber(_systemView.System);
    }

    public void DisplayWarningInfo()
    {

    }
}
