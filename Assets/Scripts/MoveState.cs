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
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        float effectiveSpeed = stateMachine.moveSpeed * Time.deltaTime;
        Vector3 moveDir = stateMachine.moveDir;
        if(stateMachine.btnJump == 1.0f)
        {
            ChangeState("jump");
        }
        Vector3 rayOrigin = transform.position + 0.5f * moveDir;
        Ray worldCheckRay = new Ray(rayOrigin, moveDir);
        bool isWorldHit = Physics.Raycast(worldCheckRay,
                                            out RaycastHit worldHitInfo,
                                            effectiveSpeed, worldMask);

        Ray worldCheckRay2 = new Ray(rayOrigin + Vector3.up, moveDir);
        bool isWorldHit2 = Physics.Raycast(worldCheckRay2,
                                            out RaycastHit worldHitInfo2,
                                            effectiveSpeed, worldMask);

        Ray worldCheckRay3 = new Ray(rayOrigin + Vector3.down, moveDir);
        bool isWorldHit3 = Physics.Raycast(worldCheckRay3,
                                            out RaycastHit worldHitInfo3,
                                            effectiveSpeed, worldMask);
        Debug.DrawRay(rayOrigin, moveDir, Color.red);
        Debug.DrawRay(rayOrigin + Vector3.up, moveDir, Color.red);
        Debug.DrawRay(rayOrigin + Vector3.down, moveDir, Color.red);
        if (isWorldHit || isWorldHit2 || isWorldHit3)
        {
            float distToMove = 0.0f;
            RaycastHit minHitInfo;
            if(worldHitInfo.distance < worldHitInfo2.distance)
                minHitInfo = worldHitInfo;
            else
                minHitInfo = worldHitInfo2;

            if(worldHitInfo3.distance < minHitInfo.distance)
                minHitInfo = worldHitInfo3;

            Vector3 surfaceNormal = minHitInfo.normal;
            Vector3 surfaceNInv = -surfaceNormal;

            Vector3 slideVel = Vector3.zero;
            if (Vector3.Dot(moveDir, -surfaceNormal) != 1.0f)
            {
                slideVel = moveDir - (Vector3.Dot(moveDir, surfaceNInv) * surfaceNInv);
            }

            transform.position += ((distToMove) * moveDir)/* + (slideVel * Time.deltaTime)*/;
        }

        else
        {
            transform.position += effectiveSpeed * moveDir;
        }

        Ray slopeCheckRay = new Ray(transform.position + (0.8f * Vector3.down), Vector3.down);
        bool isSlopeHit = Physics.Raycast(slopeCheckRay,
                            out RaycastHit slopeHitInfo,
                            0.5f,
                            worldMask);
        if (!isSlopeHit)
        {
            ChangeState("fall");
        }
        else
        {
            float moveDist = slopeHitInfo.distance;
            transform.position += (0.21f - moveDist) * Vector3.up;
        }

    }

    private MovementStateMachine stateMachine;
    private LayerMask worldMask;
}
