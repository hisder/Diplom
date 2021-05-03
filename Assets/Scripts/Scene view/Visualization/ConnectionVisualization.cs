using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(MeshCollider))]
public class ConnectionVisualization : MonoBehaviour, ISystemElement
{
    public struct ConnectionInfo
    {
        public DustCollectorVisualization[] CollectorsView;
        public Transform[] Transforms;
    }

    private LineRenderer _lineRendered;
    private MeshCollider _collider;

    private ConnectionInfo _info;
    public ConnectionInfo Info { get => _info; }

    private void Awake()
    {
        _lineRendered = GetComponent<LineRenderer>();
        _collider = GetComponent<MeshCollider>();
    }

    private void Update()
    {
        DrawConnection();
        RefreshCollider();
    }

    public void Init(DustCollectorVisualization a, DustCollectorVisualization b)
    {
        _info = new ConnectionInfo();

        _info.CollectorsView = new DustCollectorVisualization[] { a, b };

        if (a.Collector.ExitCollector == b.Collector)
        {
            _info.Transforms = new Transform[] { a.Exit, b.Enter };
        }
        else if (a.Collector.EnterCollector == b.Collector)
        {
            _info.Transforms = new Transform[] { a.Enter, b.Exit };
        }
        else
        {
            Debug.Log($"Ошибка при создании соединения {transform.name}");
        }
    }


    //ISystemElement
    public void Move(Vector3 targetPosition) { }

    public void Destroy()
    {
        Debug.Log("destroing connection");
        _info.CollectorsView[0].Collector.Disconnect(_info.CollectorsView[1].Collector);

        Destroy(gameObject);
    }

    private void DrawConnection()
    {
        _lineRendered.SetPosition(0, Info.Transforms[0].position);
        _lineRendered.SetPosition(1, Info.Transforms[1].position);
    }

    private void RefreshCollider()
    {
        Mesh mesh = new Mesh();
        _lineRendered.BakeMesh(mesh, true);
        _collider.sharedMesh = mesh;
    }
}
