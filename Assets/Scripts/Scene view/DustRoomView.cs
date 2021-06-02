using System;
using System.Collections.Generic;
using UnityEngine;

public class DustRoomView : MonoBehaviour
{
    [SerializeField] private AspirationSystemView _systemView;
    [SerializeField] private InRoomView _inRoomView;
    private DustRoom _dustRoom;
    private Vector3 _localMousePosition;

    [SerializeField] private Camera _camera;
    public Camera Camera { get => _camera; }

    [Space]
    [SerializeField] private List<DustSourceVisualization> _dustSourceVisualizations;
    [SerializeField] private List<SuctionPointVisualization> _suctionPointsVisualization;
    [SerializeField] private List<SuctionConnectionVisualization> _connectionsVisualization;

    [Space]
    [SerializeField] private GameObject _connectionBase;

    [Space]
    [SerializeField] private Transform _aspirationSystemEnter;

    [Space]
    [SerializeField] private GameObject _suckPointBase;
    [SerializeField] private GameObject _dustSourceBase;

    [Space]
    [SerializeField] private Transform _suckPointParent;
    [SerializeField] private Transform _dustSourceParent;
    [SerializeField] private Transform _connectionParent;

    private Vector3 _offset = new Vector3(0, 0, 20);

    private ISystemElement _selectedElement;
    public event Action<ISystemElement> OnSelectChanged;

    public float SuctionEfficiency { get; private set; }


    private void Start()
    {
        var ventilation = new DustCollector("vent", new float[] { 0f, 0f, 0f, 0f, 0f });
        ventilation.SetThroughput(10000);

        _dustRoom = new DustRoom(ventilation, _systemView.System);

        _connectionsVisualization = new List<SuctionConnectionVisualization>();
    }

    public void DoUpdate(Vector2 mousePosition)
    {
        _localMousePosition = (Vector3)_camera.rect.max * _camera.orthographicSize + _camera.ScreenToWorldPoint(mousePosition);

        if (Input.GetMouseButton(0))
        {
            _selectedElement = Select();
            OnSelectChanged?.Invoke(_selectedElement);
        }

        if (_selectedElement != null)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _selectedElement.Move(_localMousePosition + Vector3.forward * 20);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            var createdObject = GameObject.Instantiate(_suckPointBase, _localMousePosition + _offset, Quaternion.identity, _suckPointParent);

            var suctionPointView = createdObject.GetComponent<SuctionPointVisualization>();
            suctionPointView.Init(this);

            AddSuctionPoint(suctionPointView);

            _inRoomView.AddElement<SuctionPointVisualization>(createdObject);

            _dustRoom.AddSuctionPoint(suctionPointView.Suction);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            var createdObject = GameObject.Instantiate(_dustSourceBase, _localMousePosition + _offset, Quaternion.identity, _dustSourceParent);

            var dustSourceVisualization = createdObject.GetComponent<DustSourceVisualization>();
            dustSourceVisualization.Init(this);

            AddDustSource(dustSourceVisualization);

            _inRoomView.AddElement<DustSourceVisualization>(createdObject);

            _dustRoom.AddDustSource(dustSourceVisualization.DustSource);
        }
    }

    private void LateUpdate()
    {
        _dustRoom.Update();
        // GetSuctionEfficiency();
        CalculateSuctionEfficiency();
        _dustRoom.Reset();
    }

    public void AddDustSource(DustSourceVisualization dustSource)
    {
        _dustSourceVisualizations.Add(dustSource);
        dustSource.OnDestroy += (visualization) =>
        {
            _dustSourceVisualizations.Remove(dustSource);
        };
    }

    public void AddSuctionPoint(SuctionPointVisualization suctionPoint)
    {
        var connnectionObject = GameObject.Instantiate(_connectionBase, _connectionParent);
        var connectionView = connnectionObject.GetComponent<SuctionConnectionVisualization>();
        connectionView.Init(suctionPoint, _aspirationSystemEnter.position);
        _connectionsVisualization.Add(connectionView);

        _dustRoom.AddSuctionPoint(suctionPoint.Suction);
        suctionPoint.OnDestroy += (visualization) =>
        {
            connectionView.Destroy();
            _connectionsVisualization.Remove(connectionView);
            _suctionPointsVisualization.Remove(suctionPoint);
        };

        _suctionPointsVisualization.Add(suctionPoint);
    }

    private void CalculateSuctionEfficiency()
    {
        float originalParticalAmount;
        float suckedParticalAmount;

        originalParticalAmount = 0;
        suckedParticalAmount = 0;

        foreach (var dustSource in _dustSourceVisualizations)
        {
            originalParticalAmount += dustSource.DustSource.ParticlesPerSecondMax;
        }

        foreach (var suctionPoint in _suctionPointsVisualization)
        {
            Debug.Log(suctionPoint.Suction.CurrentLoad);
            suckedParticalAmount += suctionPoint.Suction.CurrentLoad;
        }

        Debug.Log($"{originalParticalAmount} {suckedParticalAmount} эффективность ");

        if (originalParticalAmount == 0)
        {
            SuctionEfficiency = 1;
        }
        else
        {
            SuctionEfficiency = suckedParticalAmount / originalParticalAmount;
        }

    }

    public void Select(GameObject gameObject)
    {
        _selectedElement = gameObject.GetComponent<ISystemElement>();
        OnSelectChanged?.Invoke(_selectedElement);
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

        Debug.DrawLine(ray.origin, ray.direction);
        return item;
    }
}