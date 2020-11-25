﻿using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Imp_StateMachine : MonoBehaviour
{
    private Dictionary<Type, BaseState> _availableStates;
    public BaseState currentState { get; private set; }
    public event Action<BaseState> OnStateChange;

    // Set available states for the enemy 
    public void SetStates(Dictionary<Type, BaseState> states)
    {
        _availableStates = states;
        currentState = _availableStates.Values.First();
    }

    public void Update()
    {
        var nextState = currentState?.Tick();

        if (nextState != null && nextState != currentState?.GetType())
        {
            SwitchToState(nextState);
        }

    }

    public void SwitchToState(Type nextState)
    {
        // Debug.Log("State changed to " + currentState);
        currentState.OnStateExit();
        currentState = _availableStates[nextState];
        currentState.OnStateEnter();
        OnStateChange?.Invoke(currentState);
    }
}