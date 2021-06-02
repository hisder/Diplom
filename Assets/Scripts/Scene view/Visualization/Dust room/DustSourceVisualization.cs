using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustSourceVisualization : MonoBehaviour, ISystemElement
{
    public Action<DustSourceVisualization> OnDestroy;
    [SerializeField] private GameObject _selectionView;

    [Space]
    [SerializeField] public DustSource DustSource;

    private DustRoomView _dustRoom;

    private void Awake()
    {
        DustSource = new DustSource(DustSource.ParticlesPerSecondMax,
                                    new float[] { 0f, 0f, 0f, 0f, 0f },
                                    transform.position);
    }

    private void LateUpdate()
    {
        DustSource.SetPosition(transform.position);
    }

    public void Init(DustRoomView dustRoom)
    {
        _dustRoom = dustRoom;
    }

    //ISystemElement
    public void Move(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        DustSource.SetPosition(targetPosition);
    }

    public void Destroy()
    {
        OnDestroy?.Invoke(this);

        _dustRoom.OnSelectChanged -= SelectChange;

        Destroy(gameObject);

        Debug.Log("удаление выбранного элемента");
    }

    public void SelectChange(ISystemElement selectedElement)
    {
        bool isSelected;

        isSelected = (object)selectedElement == this;
        Debug.Log(gameObject);
        _selectionView.gameObject.SetActive(isSelected);
    }
}
