using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : State
{
    private InputAction iaForward, iaBack, iaLeft, iaRight;
    private InputAction iaJump;

    public MoveState()
    {

    }

    public override void Init()
    {
        iaForward = _actionMap["forward"];
        iaBack = _actionMap["backward"];
        iaLeft = _actionMap["left"];
        iaRight = _actionMap["right"];
        iaJump = _actionMap["jump"];
    }

    public override void OnEnter()
    {
        //Debug.Log("Entering move state");
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        Vector3 movement = Vector3.zero;
        float effectiveSpeed = 10.0f * Time.deltaTime;
        
        if (iaForward.ReadValue<float>() == 1.0f)
        {
            movement += Vector3.forward;
        }
        if (iaBack.ReadValue<float>() == 1.0f)
        {
            movement += Vector3.back;
        }
        if (iaLeft.ReadValue<float>() == 1.0f)
        {
            movement += Vector3.left;
        }
        if (iaRight.ReadValue<float>() == 1.0f)
        {
            movement += Vector3.right;
        }

        if(iaJump.ReadValue<float>() == 1.0f)
        {
            ChangeState("jump");
        }

        ((LocomotionFSM)stateController).moveDir = movement.normalized;

        LayerMask worldMask = LayerMask.GetMask("world");

        Vector3 start = _transform.position + (0.5f * movement.normalized);
        Vector3 end = start + effectiveSpeed * movement.normalized;

        //bool isHorizHit = Physics.CapsuleCast(_transform.position + 0.5f * Vector3.up,
        //                                    _transform.position - 0.5f * Vector3.up,
        //                                    0.5f,
        //                                    movement.normalized,
        //                                    out RaycastHit horizHitInfo,
        //                                    effectiveSpeed,
        //                                    worldMask
        //                                    );

        bool isHorizHit = Physics.Raycast(_transform.position + 0.5f * movement.normalized,
                                    movement.normalized,
                                    out RaycastHit horizHitInfo,
                                    effectiveSpeed,
                                    worldMask
                                    );

        if (isHorizHit)
        {
            Vector3 surfaceNormal = horizHitInfo.normal;
            bool isNormalHit = Physics.Raycast(_transform.position, -surfaceNormal,
                                            out RaycastHit normalHit, 0.5f, worldMask);
            if(isNormalHit && normalHit.distance < 0.5f)
            {
                _transform.position += (0.5f - normalHit.distance) * surfaceNormal;
            }
            else
            {
                _transform.position += ((horizHitInfo.distance - 0.01f) * movement.normalized);
            }            
        }

        else
        {
            _transform.position += (effectiveSpeed * movement.normalized);
        }

        bool isHit = Physics.Raycast(_transform.position + (0.8f * Vector3.down),
                            Vector3.down,
                            out RaycastHit hitInfo,
                            0.5f,
                            worldMask
                            );
        if (!isHit)
        {
            //{Debug.Log("fall");
            ((LocomotionFSM)_stateController).playerHVel = 10.0f * movement.normalized;
            ChangeState("fall");
        }
        else
        {
            float moveDist = hitInfo.distance;
            _transform.position += (0.21f - moveDist) * Vector3.up;
        }
    }
}
