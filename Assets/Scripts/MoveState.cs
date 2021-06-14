using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : State
{
    public MoveState()
    {

    }

    public override void Init()
    {
        stateMachine = (MovementStateMachine)stateController;
        worldMask = LayerMask.GetMask("world");
    }

    public override void OnEnter()
    {
        //Debug.Log("Entering move state");
        if (stateMachine.moveDir.magnitude == 0.0f)
        {
            stateMachine.animator.CrossFadeInFixedTime("Base Layer.idle", transitionTime);
        }
        else
        {
            stateMachine.animator.CrossFadeInFixedTime("Base Layer.run", transitionTime);
        }
    }

    public override void OnExit()
    {
        //Debug.Log("exit move");
    }

    public override void Update()
    {
        if(!stateMachine.isGrounded)
        {
            ChangeState("fall");
        }

        if (stateMachine.btnJump == 1.0f)
        {
            ChangeState("jump");
            stateMachine.isGrounded = false;
            stateMachine.motor.ForceUnground();
        }

        if (stateMachine.moveDir.magnitude == 0.0f)
        {
            //stateMachine.currentSpeed -= stateMachine.deceleration * Time.fixedDeltaTime;
            if (!stateMachine.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.idle") &&
                !stateMachine.animator.IsInTransition(0))
            {
                stateMachine.animator.CrossFadeInFixedTime("Base Layer.idle", transitionTime);
            }
        }
        else
        {
            //stateMachine.currentSpeed += stateMachine.acceleration * Time.fixedDeltaTime;
            if (!stateMachine.animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.run") &&
                !stateMachine.animator.IsInTransition(0))
            {
                stateMachine.animator.CrossFadeInFixedTime("Base Layer.run", transitionTime);
            }
            stateMachine.OrientToMove();
        }
       
    }

    public override void FixedUpdate()
    {
        Vector3 wishDir = stateMachine.moveDir;
        float wishSpeed = wishDir.magnitude;
        float currSpeed = Vector3.Dot(wishDir, stateMachine.velocity);
        float addSpeed = wishSpeed - currSpeed;
        //if(addSpeed > 0.0f)
        //{
        //    float accelSpeed = stateMachine.acceleration * Time.fixedDeltaTime * wishSpeed;
        //    if (accelSpeed > addSpeed)
        //        accelSpeed = addSpeed;
        //    stateMachine.velocity += accelSpeed * wishDir;
        //}
        stateMachine.velocity = stateMachine.maxMoveSpeed * wishDir;
    }

    private MovementStateMachine stateMachine;
    private LayerMask worldMask;
    private float transitionTime = 0.15f;
}
