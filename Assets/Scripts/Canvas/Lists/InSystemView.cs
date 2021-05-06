using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InSystemView : MonoBehaviour, IElementsList
{
    [Space]
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _elementBase;
    [SerializeField] private List<AvailableElement> _allElements;

    [Space]
    [SerializeField] private Transform _collectorsParent;
    [SerializeField] private AspirationSystemView _systemView;

    public AvailableElement SelectedElement { get; private set; }


    private void Awake()
    {
        _systemView.OnSelectChanged += SetSelected;

        foreach (var element in _allElements)
        {
            element.OnSelect += ChangeSelection;
        }
    }

    public void AddCollectorView(AvailableElement uniqueElement)
    {
        var newElementObject = GameObject.Instantiate(_elementBase, _parent);

        var newAvailableElement = newElementObject.GetComponent<AvailableElement>();

        var position = new Vector3(0, 0, -1);

        newAvailableElement.DustCollector = GameObject.Instantiate(uniqueElement.DustCollector, position, Quaternion.identity, _collectorsParent);
        newAvailableElement.Init();
        newAvailableElement.OnSelect += ChangeSelection;

        var visualization = newAvailableElement.DustCollector.GetComponent<DustCollectorVisualization>();
        visualization.Init(_systemView);
        visualization.OnDestroy += RemoveElement;

        _allElements.Add(newAvailableElement);
        _systemView.AddVisualization(visualization);
    }

    public void Reset()
    {
        int index = 0;
        do
        {
            _allElements[index].Delete();
        } while (index < _allElements.Count);

        _allElements.Clear();
    }

    private void ChangeSelection(AvailableElement element)
    {
        if (SelectedElement != null)
        {
            SelectedElement.Deselect();
        }

        var dustCollectorVisualization = element.DustCollector.GetComponent<DustCollectorVisualization>();

        _systemView.Select(dustCollectorVisualization);

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

        Debug.Log(newSelect);

        var newSelectElement = _allElements.Find(x => x.DustCollector.GetComponent<DustCollectorVisualization>() == (object)newSelect);

        if (newSelectElement != null)
        {
            SelectedElement = newSelectElement;
            SelectedElement.SetSelected();
        }
    }

    private void RemoveElement(DustCollectorVisualization visualization)
    {
        var element = _allElements.Find(x => x.DustCollector.GetComponent<DustCollectorVisualization>() == visualization);

        if (element != null)
        {
            Destroy(element.gameObject);
        }

        _allElements.Remove(element);
    }

    public void RemoveNotConnected()
    {
        bool hasConnection;
        List<AvailableElement> notConnectedElements;

        notConnectedElements = new List<AvailableElement>();

        foreach (var element in _allElements)
        {
            hasConnection = element.DustCollector.GetComponent<DustCollectorVisualization>().HasConnection();

            if (!hasConnection)
            {
                notConnectedElements.Add(element);
            }
        }

        foreach (var notConnectedElement in notConnectedElements)
        {
            notConnectedElement.Delete();
        }

        _allElements.RemoveAll(x => x.DustCollector.GetComponent<DustCollectorVisualization>().HasConnection() == false);
    }

    // IElementList

    public string GetHeaderInfo()
    {
        return "Clean efficincy";
    }
    public string GetEfficiencyInfo()
    {
        var efficiencyValue = EfficiencyCalculator.CalculateEfficiency(_systemView.BaseFlow, _systemView.System.GetCleanedFlow(_systemView.BaseFlow));

        return (efficiencyValue * 100).ToString();
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
