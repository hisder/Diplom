using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DustCollectorCreater : MonoBehaviour
{
    [SerializeField] private List<TMP_InputField> _inputFields;
    [SerializeField] private TMP_InputField _nameField;

    [SerializeField] private AvailableToAddView _availablesToAdd;

    [Space]
    [SerializeField] private GameObject _dustCollectorBase;

    private void Awake()
    {
        InputFieldValidator.ApplyFullValidation(_inputFields);

        Reset();
    }

    public void Create()
    {
        DustCollector collector;
        GameObject createdCollector;
        float[] collectorEfficiency;

        int index = 0;

        collectorEfficiency = new float[5];

        foreach (var inputField in _inputFields)
        {
            collectorEfficiency[index] = float.Parse(inputField.text);
            index++;
        }

        collector = new DustCollector(_nameField.text, collectorEfficiency);

        createdCollector = GameObject.Instantiate(_dustCollectorBase, new Vector3(100f, 0f, 0f), Quaternion.identity);
        createdCollector.GetComponent<DustCollectorVisualization>().Collector = collector;

        _availablesToAdd.CreateNewVariant(createdCollector);
    }

    public void Create(DustCollector collector)
    {
        GameObject createdCollector;

        createdCollector = GameObject.Instantiate(_dustCollectorBase, new Vector3(100f, 0f, 0f), Quaternion.identity);
        createdCollector.GetComponent<DustCollectorVisualization>().Collector = collector;

        _availablesToAdd.CreateNewVariant(createdCollector);
    }

    public void Reset()
    {
        foreach (var field in _inputFields)
        {
            field.text = "0";
        }
    }

}
