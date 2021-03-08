using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionMap actionMap;

    private Vector3 position;
    private Vector3 moveDir;
    private LocomotionFSM motionFSM;

    [SerializeField]
    private float moveSpeed;

    void Start()
    {
        position = transform.position;

        MoveState moveState = new MoveState();
        moveState.actionMap = actionMap;
        moveState.transform = transform;
        moveState.Init();

        JumpState jumpState = new JumpState();
        jumpState.actionMap = actionMap;
        jumpState.transform = transform;
        jumpState.Init();

        FallState fallState = new FallState();
        fallState.actionMap = actionMap;
        fallState.transform = transform;
        fallState.Init();

        motionFSM = new LocomotionFSM();
        motionFSM.AddState("move", moveState);
        motionFSM.AddState("jump", jumpState);
        motionFSM.AddState("fall", fallState);
        motionFSM.Init(moveState);

        moveDir = Vector3.forward;
    }


    void Update()
    {
        motionFSM.Update();
    }

    void Awake()
    {
        foreach (InputAction action in actionMap)
        {
            //action.performed += OnPress;
        }
    }

    void OnEnable()
    {
        actionMap.Enable();
    }

    void OnDisable()
    {
        actionMap.Disable();   
    }

    void OnPress(InputAction.CallbackContext ctx)
    {
        string actionName = ctx.action.name;        

        if (actionName.Equals("jump"))
        {

        }

        if (actionName.Equals("shoot"))
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.transform.name);
    }
}
