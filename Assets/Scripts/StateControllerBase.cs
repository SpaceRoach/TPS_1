using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateControllerBase
{
    public StateControllerBase()
    {
        states = new Dictionary<string, State>();
    }

    public virtual void AddState(string stateName, State state)
    {
        states.Add(stateName, state);
        state.stateController = this;
    }
    public virtual void ChangeState(string stateName)
    {
        State state = states[stateName];
        activeState.OnExit();
        activeState = state;
        state.OnEnter();
    }
    public virtual void ChangeState(State newState)
    {
        activeState.OnExit();
        activeState = newState;
        newState.OnEnter();
    }
    public virtual void Update()
    {
        activeState.Update();
    }

    public virtual void Init(State state)
    {
        activeState = state;
        activeState.OnEnter();
    }

    protected State activeState;
    protected Dictionary<string, State> states;
    protected InputActionMap inputActionMap;
    protected Transform transform;
}
