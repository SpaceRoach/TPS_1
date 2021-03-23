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
        speed = 0.0f;
        hVel = ((MovementStateMachine)_stateController).playerHVel;
    }

    public override void OnExit()
    {

    }

    public override void Update()
    {
        Vector3 moveDir = stateMachine.moveDir;
        float effHSpeed = stateMachine.moveSpeedInAir * Time.deltaTime;

        float distFell = 0.0f;
        speed += (gravity * Time.deltaTime);        
        distFell += speed * Time.deltaTime;

        Ray groundCheckRay = new Ray(transform.position + Vector3.down, Vector3.down);
        bool isGroundHit = Physics.Raycast(groundCheckRay,
                            out RaycastHit groundHitInfo,
                            distFell,
                            worldMask);
        if(isGroundHit)
        {
            //Debug.Log("hit");
            distFell = groundHitInfo.distance;
            ChangeState("move");
        }
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

        transform.position += (distFell * Vector3.down);
    }

    private float gravity = 15.0f;
    private float speed = 0.0f;
    private Vector3 hVel;
    private MovementStateMachine stateMachine;
    private LayerMask worldMask;
}
