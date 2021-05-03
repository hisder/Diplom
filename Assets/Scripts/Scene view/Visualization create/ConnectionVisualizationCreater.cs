using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionVisualizationCreater : MonoBehaviour
{
    private ISystemElement _startElement;
    private ISystemElement _currentElement;

    [SerializeField] private GameObject _model;
    [SerializeField] private Transform _connnectionsParent;


    public event Action<ConnectionVisualization> OnConnectionCreat;

    public void Init(AspirationSystemView systemView)
    {
        systemView.OnSelectChanged += RefreshCurrentElement;
    }

    private void RefreshCurrentElement(ISystemElement newElement)
    {
        _currentElement = newElement;
    }

    public void Create(ISystemElement firstElement)
    {
        if (firstElement.GetType() == typeof(DustCollectorVisualization))
        {
            _startElement = firstElement;

            StartCoroutine(CreateConnectView((DustCollectorVisualization)firstElement));
        }
    }

    private IEnumerator CreateConnectView(DustCollectorVisualization firstCollectorVisualization)
    {
        bool connectedSuccessfully = false;

        Debug.Log("asd");

        yield return new WaitWhile(() => firstCollectorVisualization == (DustCollectorVisualization)_currentElement);

        Debug.Log("zxc");

        var secondCollectorVisualization = (DustCollectorVisualization)_currentElement;

        if (secondCollectorVisualization == null)
        {
            Debug.Log("cvb");
            yield break;
        }

        connectedSuccessfully = firstCollectorVisualization.Collector.TryConnect(secondCollectorVisualization.Collector, ConnectionSide.Right);

        if (connectedSuccessfully)
        {
            Debug.Log($"Соединение {connectedSuccessfully} {firstCollectorVisualization.Collector.ExitCollector} {secondCollectorVisualization.Collector.EnterCollector}");

            GameObject createdConnectionView = GameObject.Instantiate(_model, Vector3.zero, Quaternion.identity, _connnectionsParent);
            var newConnection = createdConnectionView.GetComponent<ConnectionVisualization>();
            newConnection.Init(firstCollectorVisualization, secondCollectorVisualization);

            OnConnectionCreat?.Invoke(newConnection);

            firstCollectorVisualization.Connections[0] = newConnection;
            secondCollectorVisualization.Connections[1] = newConnection;
        }
    }

}
