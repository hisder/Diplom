using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ConnectionVisualizationCreater), typeof(DustCollectorVisualizationCreater))]
public class AspirationSystemView : MonoBehaviour
{
    public AspirationSystem System { get; private set; }
    private Vector3 _localMousePosition;

    [SerializeField] private List<DustCollectorVisualization> _collectorVisualizations;
    [SerializeField] private List<ConnectionVisualization> _connectionVisualizations;

    [SerializeField] private ISystemElement _selectedElement;
    [SerializeField] private Camera _camera;
    public Camera Camera { get => _camera; }

    [Space]
    [SerializeField] private Transform _collectorsParent;

    [Space]
    [SerializeField] private GasInfoArea _gasInfo;
    [SerializeField] private GasFlow _baseFlow;


    private ConnectionVisualizationCreater _connectionCreater;
    private DustCollectorVisualizationCreater _collectorCreater;

    public event Action<ISystemElement> OnSelectChanged;
    public event Action<float> OnSystemClear;
    public event Action OnSystemDoesntComplete;


    private void Awake()
    {
        _connectionVisualizations = new List<ConnectionVisualization>();

        _connectionCreater = GetComponent<ConnectionVisualizationCreater>();
        _collectorCreater = GetComponent<DustCollectorVisualizationCreater>();

        _connectionCreater.OnConnectionCreat += ConnectionCreated;

        _connectionCreater.Init(this);
        _gasInfo.OnDataSave += UpdateFlowData;
    }

    private void Start()
    {
        _baseFlow = new GasFlow(new float[] { 1f, 1f, 1f, 1f, 1f });

        System = new AspirationSystem
                (
                    _collectorVisualizations[0].Collector,
                    _collectorVisualizations[1].Collector
                );

        for (var i = 2; i < _collectorVisualizations.Count; i++)
        {
            System.AddDustCollector(_collectorVisualizations[i].Collector);
        }
    }

    private void FixedUpdate()
    {
        if (System.CheckCompleteness())
        {
            OnSystemClear?.Invoke(EfficiencyCalculator.CalculateEfficiency(_baseFlow, System.GetCleanedFlow(_baseFlow)));
        }
        else
        {
            OnSystemDoesntComplete?.Invoke();
        }
    }

    public void AddVisualization(DustCollectorVisualization newVisualization)
    {
        _collectorVisualizations.Add(newVisualization);
        newVisualization.OnDestroy += RemoveVisualisation;
    }

    public void DoUpdate(Vector2 mousePosition)
    {
        _localMousePosition = (Vector3)_camera.rect.max * _camera.orthographicSize + _camera.ScreenToWorldPoint(mousePosition);
        Debug.DrawLine(Vector3.zero, _localMousePosition);

        if (Input.GetMouseButton(0))
        {
            _selectedElement = Select();
            OnSelectChanged?.Invoke(_selectedElement);
        }

        if (_selectedElement != null)
        {
            if (Input.GetKeyDown(KeyCode.Q)) // Move
            {
                var targetPosition = _localMousePosition + Vector3.forward * 19;

                _selectedElement.Move(targetPosition);
            }

            if (Input.GetKeyDown(KeyCode.W)) // Connect
            {
                _connectionCreater.Create(_selectedElement);
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeleteSelected();
            }
        }
    }

    private void UpdateFlowData(float[] newData)
    {
        _baseFlow = new GasFlow(newData);
    }

    private void ConnectionCreated(ConnectionVisualization newConnection)
    {
        _connectionVisualizations.Add(newConnection);
    }

    private void RemoveVisualisation(DustCollectorVisualization visualization)
    {
        _collectorVisualizations.Remove(visualization);
    }

    private void DeleteSelected()
    {
        if (_selectedElement.GetType() == typeof(ConnectionVisualization))
        {
            var item = _connectionVisualizations.Find(x => x == (object)_selectedElement);
            _connectionVisualizations.Remove(item);
        }

        _selectedElement.Destroy();
    }

    public void Select(DustCollectorVisualization selectedElement)
    {
        _selectedElement = selectedElement;
        OnSelectChanged?.Invoke(selectedElement);
    }

    private ISystemElement Select()
    {
        Ray ray;
        RaycastHit hitInfo;

        ISystemElement item = null;

        ray = new Ray(_localMousePosition, Vector3.forward * 100);

        if (Physics.Raycast(ray, out hitInfo))
        {
            item = hitInfo.transform.GetComponent<ISystemElement>();
        }

        return item;
    }
}
