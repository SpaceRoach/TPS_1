using UnityEngine;

public class MovementStateMachine : StateControllerBase
{
    public override void Init(State state)
    {
        base.Init(state);
        cameraAimDir = Vector3.forward;
        playerAimDir = cameraAimDir;
        playerStrafeDir = Vector3.Cross(Vector3.up, playerAimDir);
    }

    public override void ReadInput()
    {
        btnFwd = inputActionMap["forward"].ReadValue<float>();
        btnLeft = inputActionMap["left"].ReadValue<float>();
        btnRight = inputActionMap["right"].ReadValue<float>();
        btnBack = inputActionMap["backward"].ReadValue<float>();
        btnJump = inputActionMap["jump"].ReadValue<float>();
    }

    public override void Update()
    {
        cameraAimDir = cameraAimDir.normalized;

        playerAimDir = cameraAimDir;
        playerAimDir.y = 0.0f;
        playerAimDir = playerAimDir.normalized;

        playerStrafeDir = Vector3.Cross(Vector3.up, playerAimDir);

        float deltaFwd = (btnFwd - btnBack);
        float deltaSide = (btnRight - btnLeft);

        moveDir = (deltaFwd * playerAimDir + deltaSide * playerStrafeDir).normalized;
        base.Update();
    }
         
    public float btnFwd;
    public float btnLeft;
    public float btnRight;
    public float btnBack;
    public float btnJump;

    public Vector3 cameraAimDir;
    public Vector3 playerAimDir;
    public Vector3 playerStrafeDir;
    public Vector3 moveDir;
    public float moveSpeed = 10.0f;
    public float moveSpeedInAir = 5.0f;
    public Vector3 playerHVel;
}
