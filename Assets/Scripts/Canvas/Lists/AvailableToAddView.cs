using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableToAddView : MonoBehaviour
{
    [SerializeField] private GameObject _elementView;
    [SerializeField] private Transform _elementParentObject;
    [SerializeField] private InSystemView _systemViewList;

    [Space]
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _elementBase;
    [SerializeField] private List<AvailableElement> _availableElements;

    [Space]
    [SerializeField] private DustCollectorCreater _collectorCreater;


    public AvailableElement SelectedElement { get; private set; }

    private void Awake()
    {
        _collectorCreater.Create(new DustCollector("Test 100", new float[] { 1f, 1f, 1f, 1f, 1f }));
        _collectorCreater.Create(new DustCollector("Test 0", new float[] { 0f, 0f, 0f, 0f, 0f }));
    }

    public void CreateNewVariant(GameObject collector)
    {
        var newElement = GameObject.Instantiate(_elementBase, _parent).GetComponent<AvailableElement>();

        newElement.Init(collector);
        newElement.OnSelect += ChangeSelection;
        _availableElements.Add(newElement);
    }

    public void AddSelectedToSystem(Transform collectorsViewParent)
    {
        if (SelectedElement != null)
        {
            _systemViewList.AddCollectorView(SelectedElement);
        }
    }

    public void RemoveSelected()
    {
        if (SelectedElement != null)
        {
            Destroy(SelectedElement.DustCollector);
            Destroy(SelectedElement.gameObject);

            _availableElements.Remove(SelectedElement);
        }
    }

    private void ChangeSelection(AvailableElement element)
    {
        if (SelectedElement != null)
        {
            SelectedElement.Deselect();
        }

        SelectedElement = element;
    }
}
