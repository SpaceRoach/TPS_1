using UnityEngine;

public class JumpState : State
{
    public override void Init()
    {
        stateMachine = (MovementStateMachine)stateController;
        worldMask = LayerMask.GetMask("world");
    }

    public override void OnEnter()
    {
        //Debug.Log("entering jump state ");
        stateMachine.velocity += stateMachine.jumpInitialSpeed * Vector3.up;
        stateMachine.animator.CrossFadeInFixedTime("jump", 0.1f);
    }

    public override void OnExit()
    {
        stateMachine.velocity.y = 0.0f;
    }

    public override void Update()
    {
        if (stateMachine.isGrounded)
        {
            ChangeState("move");
        }
        prevYSpeed = currentYSpeed;
        currentYSpeed = stateMachine.velocity.y;
        //if (currentYSpeed > 0.0f)
        //{
        //    stateMachine.animator.CrossFadeInFixedTime("jump_ascend", 1.25f);
        //}
        //else if(currentYSpeed < 0.0f)
        //{
        //    if(prevYSpeed > 0.0f)
        //        stateMachine.animator.CrossFadeInFixedTime("jump_peak", 0.25f);
        //    //else
        //        //stateMachine.animator.CrossFade("jump_descend", 2.0f);
        //}
        stateMachine.OrientToMove();
    }



    public override void FixedUpdate()
    {
        if (stateMachine.moveDir.magnitude == 0.0f)
        {
            stateMachine.currentSpeed -= stateMachine.deceleration * Time.fixedDeltaTime;
        }
        else
        {
            stateMachine.currentSpeed += (stateMachine.airAccel) * Time.fixedDeltaTime;
            Vector3 horizontalVel = stateMachine.currentSpeed * stateMachine.moveDir;
            stateMachine.velocity.x = horizontalVel.x;
            stateMachine.velocity.z = horizontalVel.z;
        }
        stateMachine.velocity.y -= stateMachine.gravity * Time.fixedDeltaTime;
    }
    private MovementStateMachine stateMachine;
    private LayerMask worldMask;
    private float prevYSpeed;
    private float currentYSpeed;
}
