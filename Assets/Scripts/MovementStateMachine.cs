using UnityEngine;
using KinematicCharacterController;

public class MovementStateMachine : StateControllerBase
{
    public override void Init(State state)
    {
        base.Init(state);
        cameraAimDir = Vector3.forward;
        moveDir = faceDirection = cameraAimDir;
        strafeDirection = Vector3.Cross(Vector3.up, moveDir);
    }

    public override void ReadInput()
    {
        btnFwd = inputActionMap["forward"].ReadValue<float>();
        btnLeft = inputActionMap["left"].ReadValue<float>();
        btnRight = inputActionMap["right"].ReadValue<float>();
        btnBack = inputActionMap["backward"].ReadValue<float>();
        btnJump = inputActionMap["jump"].ReadValue<float>();
        btnDbg = inputActionMap["debug"].ReadValue<float>();
    }

    public override void Update()
    {
        ReadInput();
        base.Update();
    }

    public override void FixedUpdate()
    {
        //cameraAimDir = cameraAimDir.normalized;
        float moveFwdDelta = (btnFwd - btnBack);
        float moveSideDelta = (btnRight - btnLeft);
        moveDir = Vector3.zero;
        if (moveFwdDelta != 0.0f || moveSideDelta != 0.0f)
        {
            //strafeDirection = Vector3.Cross(Vector3.up, cameraAimDir);
            //strafeDirection = strafeDirection.normalized;
            Vector3 cameraDirXZ = cameraAimDir;
            cameraDirXZ.y = 0.0f;
            moveDir = (moveFwdDelta * cameraDirXZ + moveSideDelta * strafeDirection);
            moveDir.y = 0.0f;
            moveDir = moveDir.normalized;
            //moveDir = faceDirection;
        }

        //Quaternion targetRotation = Quaternion.LookRotation(faceDirection);
        //transform.rotation = targetRotation;// Quaternion.Slerp(transform.rotation, targetRotation, rotationT);

        base.FixedUpdate();
        currentSpeed = Mathf.Clamp(currentSpeed, 0.0f, maxMoveSpeed);
    }

    public void OrientToMove()
    {
        if (moveDir.magnitude != 0.0f)
        {
            float angleVec = Vector3.SignedAngle(faceDirection, moveDir, Vector3.up);
            float sign = Mathf.Sign(angleVec);
            float angleTurn = sign * turnSpeed * Time.deltaTime;
            if (Mathf.Abs(angleVec) < Mathf.Abs(angleTurn))
            {
                angleTurn = angleVec;
            }
            faceDirection = Quaternion.AngleAxis(angleTurn, Vector3.up) * faceDirection;
            orientation = Quaternion.LookRotation(faceDirection);
        }
    }

    public float btnFwd;
    public float btnLeft;
    public float btnRight;
    public float btnBack;
    public float btnJump;
    public float btnDbg;

    public Vector3 cameraAimDir;
    public Vector3 faceDirection;
    public Vector3 strafeDirection;
    public Vector3 moveDir;
    public Vector3 velocity;
    public Quaternion orientation;

    public float maxMoveSpeed = 8.0f;
    public float airAccel = 20.0f;
    public float jumpInitialSpeed = 10.0f;
    public float gravity = 25.0f;
    public float acceleration = 55.0f;
    public float deceleration = 60.0f;
    public float currentSpeed = 0.0f;
    public float turnSpeed = 500.0f;

    public bool isGrounded;
    public bool isTurning = false;

    public KinematicCharacterMotor motor {
        get => _motor;
        set => _motor = value;
    }
    private KinematicCharacterMotor _motor;

    private float rotationT;
    public Animator animator;

}
