using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private InSystemView _inSystemListView;

    private void Awake()
    {
        Screen.SetResolution(800, 780, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            _inSystemListView.RemoveSelected();
        }
    }

}
