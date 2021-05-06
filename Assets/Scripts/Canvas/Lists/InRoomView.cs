using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRoomView : MonoBehaviour, IElementsList
{
    [Space]
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _elementBase;
    [SerializeField] private List<RoomAvailableElement> _allElements;

    [SerializeField] private DustRoomView _roomView;

    public RoomAvailableElement SelectedElement { get; private set; }


    private void Awake()
    {
        foreach (var element in _allElements)
        {
            element.OnSelect += ChangeSelection;
        }
        _roomView.OnSelectChanged += SetSelected;
    }


    public void AddElement<T>(GameObject elementView)
    {
        var elementObject = GameObject.Instantiate(_elementBase, _parent);

        var newAvailableElement = elementObject.GetComponent<RoomAvailableElement>();

        newAvailableElement.Init<T>(elementView);

        if (typeof(T) == typeof(DustSourceVisualization))
        {
            _roomView.OnSelectChanged += elementView.GetComponent<DustSourceVisualization>().SelectChange;
            elementView.GetComponent<DustSourceVisualization>().OnDestroy += (visualization) =>
            {
                _allElements.Remove(newAvailableElement);
                Destroy(elementObject);
            };
        }

        if (typeof(T) == typeof(SuctionPointVisualization))
        {
            _roomView.OnSelectChanged += elementView.GetComponent<SuctionPointVisualization>().SelectChange;
            elementView.GetComponent<SuctionPointVisualization>().OnDestroy += (visualization) =>
            {
                _allElements.Remove(newAvailableElement);
                Destroy(elementObject);
            };
        }

        newAvailableElement.OnSelect += ChangeSelection;
        _allElements.Add(newAvailableElement);
    }

    private void ChangeSelection(RoomAvailableElement element)
    {
        if (SelectedElement != null)
        {
            SelectedElement.Deselect();
        }

        _roomView.Select(element.Element);

        SelectedElement = element;
    }

    private void SetSelected(ISystemElement newSelect)
    {
        if (SelectedElement != null)
        {
            SelectedElement.Deselect();
        }

        if (newSelect == null)
        {
            SelectedElement = null;
            return;
        }

        var newSelectElement = _allElements.Find(x => x.Element.GetComponent<ISystemElement>() == (object)newSelect);

        if (newSelectElement != null)
        {
            SelectedElement = newSelectElement;
            SelectedElement.SetSelected();
        }
    }

    // buttons
    public void RemoveAllDustSources()
    {
        RemoveAll<DustSourceVisualization>();
    }

    public void RemoveAllSuctionPoints()
    {
        RemoveAll<SuctionPointVisualization>();
    }

    private void RemoveAll<T>()
    {
        var removebleElements = _allElements.FindAll(dustSource => dustSource.Element.GetComponent<ISystemElement>().GetType() == typeof(T));

        foreach (var element in removebleElements)
        {
            element.Delete();
        }
    }

    // IElementList

    public string GetHeaderInfo()
    {
        return "Suction efficincy";
    }
    public string GetEfficiencyInfo()
    {
        return (_roomView.SuctionEfficiency * 100).ToString();
    }

    public void RemoveSelected()
    {
        if (SelectedElement != null)
        {
            SelectedElement.Delete();
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
