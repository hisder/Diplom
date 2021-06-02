using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GasInfoArea : MonoBehaviour
{
    public event Action<float[]> OnDataSave;

    [SerializeField] private List<TMP_InputField> _inputAreas;
    [SerializeField] private List<TextMeshProUGUI> _actualValueText;

    private float[] _savedInfo;


    void Start()
    {
        if (_inputAreas.Count != _actualValueText.Count)
        {
            Debug.Log($"Error {this}");
        }

        InputFieldValidator.ApplyFullValidation(_inputAreas);

        foreach (var inputField in _inputAreas)
        {
            inputField.text = "1";
        }

        _savedInfo = new float[_actualValueText.Count];
    }

    public void SaveData()
    {
        for (int i = 0; i < 5; i++)
        {
            _savedInfo[i] = float.Parse(_inputAreas[i].text);
        }

        OnDataSave?.Invoke(_savedInfo);
    }
}
