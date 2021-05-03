using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustCollectorVisualization : MonoBehaviour, ISystemElement
{
    public Action<DustCollectorVisualization> OnDestroy;
    [SerializeField] public DustCollector Collector;
    [SerializeField] private GameObject _selectionView;

    [Space]
    public Transform Enter;
    public Transform Exit;

    public bool IsStatic;

    public ConnectionVisualization[] Connections { get; private set; }

    private AspirationSystemView _systemView;

    public enum DustCollectorType
    {
        Enter,
        Midl,
        Exit
    }
    public DustCollectorType Type = DustCollectorType.Midl;

    private void Awake()
    {
        switch (Type)
        {
            case DustCollectorType.Enter:
                Collector = new EnterDustCollector("Enter", new float[] { 0f, 0f, 0f, 0f, 0f });
                break;

            case DustCollectorType.Exit:
                Collector = new ExitDustCollector("Exit", new float[] { 0f, 0f, 0f, 0f, 0f });
                break;
        }

        Connections = new ConnectionVisualization[2];
    }

    public void Init(AspirationSystemView systemView)
    {
        _systemView = systemView;
        _systemView.OnSelectChanged += SelectChange;
    }

    public void SelectChange(ISystemElement selectedElement)
    {
        bool isSelected;

        isSelected = (object)selectedElement == this;
        Debug.Log(gameObject);
        _selectionView.gameObject.SetActive(isSelected);
    }

    public bool HasConnection()
    {
        return Connections[0] != null || Connections[1] != null;
    }

    //ISystemElement

    public void Move(Vector3 targetPosition)
    {
        if (!IsStatic)
        {
            transform.position = targetPosition;
        }
    }

    public void Destroy()
    {
        if (IsStatic)
        {
            return;
        }

        if (Collector.ExitCollector != null)
        {
            Collector.Disconnect(Collector.ExitCollector);
        }

        if (Collector.EnterCollector != null)
        {
            Collector.Disconnect(Collector.EnterCollector);
        }

        for (var i = 0; i < 2; i++)
        {
            if (Connections[i] != null)
            {
                Debug.Log("connection");
                Connections[i].Destroy();
            }
        }

        OnDestroy?.Invoke(this);

        _systemView.OnSelectChanged -= SelectChange;

        Destroy(gameObject);
    }
}
