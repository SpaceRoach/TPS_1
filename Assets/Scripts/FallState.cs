using UnityEngine;
using UnityEngine.InputSystem;

public class FallState : State
{
    public override void Init()
    {
        stateMachine = (MovementStateMachine)stateController;
        worldMask = LayerMask.GetMask("world");
    }

    public override void OnEnter()
    {
        //Debug.Log("enter fall state");
        stateMachine.animator.CrossFadeInFixedTime("fall", 0.25f);
    }

    public override void OnExit()
    {
        //Debug.Log("exit fall state");
    }

    public override void Update()
    {
        if(stateMachine.isGrounded)
        {
            ChangeState("move");
        }
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
            stateMachine.currentSpeed += stateMachine.airAccel * Time.fixedDeltaTime;
            Vector3 horizontalVel = stateMachine.currentSpeed * stateMachine.moveDir;
            stateMachine.velocity.x = horizontalVel.x;
            stateMachine.velocity.z = horizontalVel.z;
        }
        stateMachine.velocity.y -= stateMachine.gravity * Time.fixedDeltaTime;
    }
    private MovementStateMachine stateMachine;
    private LayerMask worldMask;
}
