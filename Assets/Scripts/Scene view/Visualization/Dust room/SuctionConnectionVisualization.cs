using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(MeshCollider))]
public class SuctionConnectionVisualization : MonoBehaviour
{
    private LineRenderer _lineRendered;
    private MeshCollider _collider;

    private Transform _connectedPoint;

    private void Awake()
    {
        _lineRendered = GetComponent<LineRenderer>();
        _collider = GetComponent<MeshCollider>();
    }

    public void Init(SuctionPointVisualization a, Vector2 aspirationSystemEnter)
    {
        _connectedPoint = a.transform;
        _lineRendered.SetPosition(1, aspirationSystemEnter);
    }

    private void Update()
    {
        _lineRendered.SetPosition(0, _connectedPoint.position);
        RefreshCollider();
    }

    private void RefreshCollider()
    {
        Mesh mesh = new Mesh();
        _lineRendered.BakeMesh(mesh, true);
        _collider.sharedMesh = mesh;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
