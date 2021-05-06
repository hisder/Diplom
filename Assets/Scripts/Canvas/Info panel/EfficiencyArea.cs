using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EfficiencyArea : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _headerTextArea;
    [SerializeField] private TextMeshProUGUI _efficiencyTextArea;

    [Space]
    [SerializeField] private UI _UI;

    [SerializeField] private AspirationSystemView _systemView;

    private float _cleanEfficiency;
    private float _suckEfficiency;

    private void Awake()
    {
        _systemView.OnSystemClear += DisplatEfficiency;

        _systemView.OnSystemDoesntComplete += () => { _efficiencyTextArea.text = "NAN"; };

    }

    public void RefreshDisplayerInfo(string headerText, string efficiencyText)
    {
        _headerTextArea.text = headerText;
        _efficiencyTextArea.text = efficiencyText;
    }

    private void DisplatEfficiency(float efficiency)
    {
        _efficiencyTextArea.text = $"{efficiency.ToString()}%";
    }
}
