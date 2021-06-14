using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using KinematicCharacterController;

public class PlayerController : MonoBehaviour, ICharacterController
{
    public InputActionMap actionMap;
    public KinematicCharacterMotor motor;

    [Header("Animation")]
    public List<AnimationClip> animationClips;
    public Animator animator;

    private Vector3 position;
    private Vector3 moveDir;
    private MovementStateMachine movementStateMachine;

    public ArcBallCamera arcCamera;

    [SerializeField]
    private float moveSpeed;

    public Vector3 debugVelocity;

    void Awake()
    {
        motor.CharacterController = this;
        foreach (InputAction action in actionMap)
        {
            //action.performed += OnPress;
        }
    }

    void Start()
    {
        position = transform.position;

        MoveState moveState = new MoveState();
        JumpState jumpState = new JumpState();
        FallState fallState = new FallState();

        movementStateMachine = new MovementStateMachine();
        movementStateMachine.inputActionMap = actionMap;
        movementStateMachine.transform = transform;
        movementStateMachine.motor = motor;
        movementStateMachine.animator = animator;
        movementStateMachine.AddState("move", moveState);
        movementStateMachine.AddState("jump", jumpState);
        movementStateMachine.AddState("fall", fallState);
        movementStateMachine.Init(moveState);
        
        moveDir = Vector3.forward;
    }

    void FixedUpdate()
    {
        movementStateMachine.FixedUpdate();
    }

    void Update()
    {
        movementStateMachine.ReadInput();
        movementStateMachine.cameraAimDir = arcCamera.GetFacingDirection().normalized;
        movementStateMachine.strafeDirection = arcCamera.GetSideDirection().normalized;
        movementStateMachine.Update();
        if(movementStateMachine.btnDbg == 1.0f)
        {
            Vector3 a = movementStateMachine.faceDirection;
            Vector3 b = movementStateMachine.moveDir;
            a.y = 0; b.y = 0;
            float angle = Vector3.SignedAngle(a, b, Vector3.up);
            Debug.Log("facedir " + a + "angle " + angle + ", dot: " + Vector3.Dot(a,b));
        }
    }

    void OnEnable()
    {
        actionMap.Enable();
    }

    void OnDisable()
    {
        actionMap.Disable();
    }

    void OnPress(InputAction.CallbackContext ctx)
    {
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {

    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {

    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {

    }

    public void PostGroundingUpdate(float deltaTime)
    {
        movementStateMachine.isGrounded = motor.GroundingStatus.IsStableOnGround;
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {

    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        currentRotation = movementStateMachine.orientation;
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        currentVelocity = movementStateMachine.velocity;
    }
}