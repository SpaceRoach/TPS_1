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
        //Debug.Log("Entering jump state");
        jumpSpeed = 10.0f;
        hVel = stateMachine.playerHVel;
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        Vector3 moveDir = stateMachine.moveDir;
        float effHSpeed = stateMachine.moveSpeedInAir * Time.deltaTime;
        float jumpSign = Mathf.Sign(jumpSpeed);
        Vector3 vertDisplacement = jumpSpeed * Time.deltaTime * Vector3.up;

        Vector3 rayOrig = transform.position + (jumpSign * Vector3.up);
        Ray groundCheckRay = new Ray(rayOrig,
                                    jumpSign * Vector3.up);

        bool isGroundHit = Physics.Raycast(groundCheckRay, 
                                    out RaycastHit groundHitInfo, 
                                    Mathf.Abs(jumpSpeed) * Time.deltaTime,
                                    worldMask);
        if (isGroundHit)
        {
            //Debug.Log("hit " + hitInfo.transform.name + " at " + jumpSpeed);
            vertDisplacement = groundHitInfo.distance * Mathf.Sign(jumpSpeed) * Vector3.up;
            ChangeState("move");
        }
        transform.position += vertDisplacement;
        Vector3 rayOrigin = transform.position + 0.5f * moveDir;
        Ray worldCheckRay = new Ray(rayOrigin, moveDir);
        bool isWorldHit = Physics.Raycast(worldCheckRay,
                                            out RaycastHit worldHitInfo,
                                            effHSpeed, worldMask);

        Ray worldCheckRay2 = new Ray(rayOrigin + Vector3.up, moveDir);
        bool isWorldHit2 = Physics.Raycast(worldCheckRay2,
                                            out RaycastHit worldHitInfo2,
                                            effHSpeed, worldMask);

        Ray worldCheckRay3 = new Ray(rayOrigin + Vector3.down, moveDir);
        bool isWorldHit3 = Physics.Raycast(worldCheckRay3,
                                            out RaycastHit worldHitInfo3,
                                            effHSpeed, worldMask);
        Debug.DrawRay(transform.position + 0.5f * moveDir, moveDir, Color.red);
        Debug.DrawRay(transform.position + Vector3.up, moveDir, Color.red);
        Debug.DrawRay(transform.position + Vector3.down, moveDir, Color.red);
        if (isWorldHit || isWorldHit2 || isWorldHit3)
        {
            float distToMove = Mathf.Min(Mathf.Min(worldHitInfo.distance, worldHitInfo2.distance), worldHitInfo3.distance);
            transform.position += (distToMove) * moveDir;
        }

        else
        {
            transform.position += effHSpeed * moveDir;
        }

        jumpSpeed -= gravity * Time.deltaTime;
    }

    private MovementStateMachine stateMachine;
    private float gravity = 15.0f;
    private float jumpSpeed = 50.0f;
    private Vector3 hVel;
    LayerMask worldMask;
}
