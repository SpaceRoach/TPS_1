using UnityEngine;
using UnityEngine.InputSystem;

public abstract class State
{
    protected InputActionMap _actionMap;

    public InputActionMap actionMap
    {
        set { _actionMap = value;  }
    }

    protected Transform _transform;

    public Transform transform
    {
        set
        {
            _transform = value;
        }
    }

    protected StateControllerBase _stateController;

    public StateControllerBase stateController
    {
        get => _stateController;
        set => _stateController = value;
    }

    public virtual void Init()
    {

    }
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void Update();
    protected void ChangeState(string stateName)
    {
        _stateController.ChangeState(stateName);
    }
}
