using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AvailableElement : MonoBehaviour
{
    public Action<AvailableElement> OnSelect;

    [SerializeField] public GameObject DustCollector { get; set; }

    [SerializeField] private Color _selectColor;
    [SerializeField] private Color _baseColor;
    [SerializeField] private Image _buttonImage;

    [Space]
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _efficiencyValue;


    public void Init(GameObject dustCollectorGameobject)
    {
        var collectorVisualisation = dustCollectorGameobject.GetComponent<DustCollectorVisualization>();

        if (collectorVisualisation == null)
        {
            Debug.Log($"CollectorVisualisation == null {this}");
            return;
        }

        DustCollector = dustCollectorGameobject;

        _name.text = collectorVisualisation.Collector.Name;

        _efficiencyValue.text = $"{EfficiencyCalculator.CalculateEfficiency(collectorVisualisation.Collector).ToString()}%";
    }

    public void Delete()
    {
        if (DustCollector != null)
        {
            DustCollector.GetComponent<DustCollectorVisualization>().Destroy();
        }
        Destroy(gameObject);
    }

    public void Init()
    {
        _name.text = DustCollector.GetComponent<DustCollectorVisualization>().Collector.Name;
    }

    public void Deselect()
    {
        _buttonImage.color = _baseColor;
    }

    public void SetSelected()
    {
        _buttonImage.color = _selectColor;
    }

    public void Select()
    {
        _buttonImage.color = _selectColor;
        OnSelect?.Invoke(this);
    }
}
