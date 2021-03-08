using UnityEngine;
using UnityEngine.InputSystem;

public class JumpState : State
{
    private InputAction iaForward, iaBack, iaLeft, iaRight;

    private float gravity = 15.0f;
    private float jumpSpeed = 50.0f;
    private Vector3 hVel;

    public override void Init()
    {
        iaForward = _actionMap["forward"];
        iaBack = _actionMap["backward"];
        iaLeft = _actionMap["left"];
        iaRight = _actionMap["right"];
    }

    public override void OnEnter()
    {
        //Debug.Log("Entering jump state");
        jumpSpeed = 10.0f;
        hVel = ((LocomotionFSM)stateController).playerHVel;
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        Vector3 horizMovement = Vector3.zero;

        if (iaForward.ReadValue<float>() == 1.0f)
        {
            horizMovement += Vector3.forward;
        }
        if (iaBack.ReadValue<float>() == 1.0f)
        {
            horizMovement += Vector3.back;
        }
        if (iaLeft.ReadValue<float>() == 1.0f)
        {
            horizMovement += Vector3.left;
        }
        if (iaRight.ReadValue<float>() == 1.0f)
        {
            horizMovement += Vector3.right;
        }

        LayerMask worldMask = LayerMask.GetMask("world");
        bool isHit = Physics.Raycast(_transform.position + Vector3.down, 
                                    Mathf.Sign(jumpSpeed) * Vector3.up, 
                                    out RaycastHit hitInfo, 
                                    Mathf.Abs(jumpSpeed) * Time.deltaTime,
                                    worldMask
                                    );
        Vector3 moveDist = jumpSpeed * Time.deltaTime * Vector3.up;
        if (isHit)
        {
            //Debug.Log("hit " + hitInfo.transform.name + " at " + jumpSpeed);
            moveDist = hitInfo.distance * Mathf.Sign(jumpSpeed) * Vector3.up;
            ChangeState("move");
        }
        _transform.position += (moveDist + (5.0f * Time.deltaTime * horizMovement.normalized));
        jumpSpeed -= gravity * Time.deltaTime;
    }
}
