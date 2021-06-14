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
        state.actionMap = inputActionMap;
        state.transform = transform;
        state.Init();
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

    public virtual void FixedUpdate()
    {
        activeState.FixedUpdate();
    }

    public virtual void Init(State state)
    {
        activeState = state;
        activeState.OnEnter();
    }

    public virtual void ReadInput()
    {

    }

    public InputActionMap inputActionMap
    {
        get => _inputActionMap;
        set => _inputActionMap = value;
    }

    public Transform transform
    {
        get => _transform;
        set => _transform = value;
    }

    protected State activeState;
    protected Dictionary<string, State> states;
    protected InputActionMap _inputActionMap;
    protected Transform _transform;
}
