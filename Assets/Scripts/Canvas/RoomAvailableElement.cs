using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomAvailableElement : MonoBehaviour
{
    public Action<RoomAvailableElement> OnSelect;

    [SerializeField] public GameObject Element { get; set; }

    [SerializeField] private Color _selectColor;
    [SerializeField] private Color _baseColor;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private TMP_Text _textArea;

    public enum ElementType
    {
        DustSourceVisualization,
        SuctionPointVisualization
    }

    public ElementType HoldElementType { get; private set; }

    public void Init<T>(GameObject roomElement)
    {
        var roomElementVisualization = roomElement.GetComponent<T>();

        if (roomElementVisualization == null)
        {
            Debug.Log($"CollectorVisualisation == null {this}");
            return;
        }

        Element = roomElement;

        if (typeof(T) == typeof(DustSourceVisualization))
        {
            HoldElementType = ElementType.DustSourceVisualization;
            _textArea.text = "Dust source";
        }

        if (typeof(T) == typeof(SuctionPointVisualization))
        {
            HoldElementType = ElementType.SuctionPointVisualization;
            _textArea.text = "Suction point";
        }
    }

    public void Delete()
    {
        if (Element != null)
        {
            Element.GetComponent<ISystemElement>().Destroy();
        }
        Destroy(gameObject);
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
