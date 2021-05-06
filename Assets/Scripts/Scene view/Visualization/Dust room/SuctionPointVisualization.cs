using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuctionPointVisualization : MonoBehaviour, ISystemElement
{
    public Action<SuctionPointVisualization> OnDestroy;
    [SerializeField] private GameObject _suctionOptimalRadius;
    [SerializeField] private GameObject _suctionMaxRadius;
    [SerializeField] private GameObject _selectionView;

    [Space]
    [SerializeField] public SuctionPoint Suction;

    private DustRoomView _dustRoom;

    private void Awake()
    {
        Suction = new SuctionPoint(transform.position, Suction.SuctionOptimalDistance, Suction.SuctionMaxDistance, 10000);
        RefreshCircleScale(_suctionOptimalRadius, Suction.SuctionOptimalDistance);
        RefreshCircleScale(_suctionMaxRadius, Suction.SuctionMaxDistance);
    }

    public void Init(DustRoomView dustRoom)
    {
        _dustRoom = dustRoom;
    }

    private void LateUpdate()
    {
        Suction.SetPosition(transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Suction.SuctionMaxDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Suction.SuctionOptimalDistance);
    }

    public void Move(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    public void Destroy()
    {
        OnDestroy?.Invoke(this);

        _dustRoom.OnSelectChanged -= SelectChange;

        Destroy(gameObject);
    }

    private void RefreshCircleScale(GameObject circle, float newScale)
    {
        circle.transform.localScale = new Vector3(newScale, newScale, 1);
    }

    public void SelectChange(ISystemElement selectedElement)
    {
        bool isSelected;

        isSelected = (object)selectedElement == this;
        Debug.Log(gameObject);
        _selectionView.gameObject.SetActive(isSelected);
    }
}
