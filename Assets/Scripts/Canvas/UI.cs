using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UI : MonoBehaviour
{
    [SerializeField] private List<Component> _inStateHolders;
    [SerializeField] private EfficiencyArea _efficiencyArea;

    private List<IElementsList> _states;

    private int _currentStateIndex;

    private void Awake()
    {
        Screen.SetResolution(800, 780, false);

        _states = new List<IElementsList>();


        foreach (var component in _inStateHolders)
        {
            var element = component.GetComponent<IElementsList>();

            if (element != null)
            {
                _states.Add(element);
            }
        }

        foreach (var state in _states)
        {
            state.Deactivate();
        }
    }

    private void Start()
    {
        _currentStateIndex = 0;
        _states[_currentStateIndex].Activate();
        _efficiencyArea.RefreshDisplayerInfo(_states[_currentStateIndex].GetHeaderInfo(),
                                             _states[_currentStateIndex].GetEfficiencyInfo());
    }

    private void FixedUpdate()
    {
        _efficiencyArea.RefreshDisplayerInfo(_states[_currentStateIndex].GetHeaderInfo(),
                                             _states[_currentStateIndex].GetEfficiencyInfo());
    }

    public void ChangeState(int stateindex)
    {
        _states[_currentStateIndex].Deactivate();

        _currentStateIndex = stateindex;

        _states[_currentStateIndex].Activate();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            _states[_currentStateIndex].RemoveSelected();
        }
    }
}
