using UnityEngine;
using UnityEngine.InputSystem;

public class FallState : State
{
    private float gravity = 15.0f;
    private float speed = 0.0f;
    private InputAction iaForward, iaBack, iaLeft, iaRight;
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
        speed = 0.0f;
        hVel = ((LocomotionFSM)_stateController).playerHVel;
    }

    public override void OnExit()
    {

    }

    public override void Update()
    {
        Vector3 movement = Vector3.zero;
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
        LayerMask worldMask = LayerMask.GetMask("world");
        speed += (gravity * Time.deltaTime);
        float distFell = 0.0f;
        distFell += speed * Time.deltaTime;
        bool isHit = Physics.Raycast(_transform.position + Vector3.down,
                            Vector3.down,
                            out RaycastHit hitInfo,
                            distFell,
                            worldMask
                            );
        if(isHit)
        {
            //Debug.Log("hit");
            distFell = hitInfo.distance;
            ChangeState("move");
        }
        _transform.position += (distFell * Vector3.down) + (10.0f * movement.normalized * Time.deltaTime);
    }
}
