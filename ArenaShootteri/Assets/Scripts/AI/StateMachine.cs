using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
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

    // Update is called once per frame
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
            currentState = _availableStates[nextState];
            Debug.Log("State changed to " + currentState.ToString());
            OnStateChange?.Invoke(currentState);
        }
    

    
}
