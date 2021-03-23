using System;
using UnityEngine;
using UnityEngine.InputSystem;
using KinematicCharacterController;

public class PlayerController : MonoBehaviour, ICharacterController
{
    public InputActionMap actionMap;
    public KinematicCharacterMotor motor;

    private Vector3 position;
    private Vector3 moveDir;
    private MovementStateMachine movementStateMachine;

    [SerializeField]
    private float moveSpeed;

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
        movementStateMachine.AddState("move", moveState);
        movementStateMachine.AddState("jump", jumpState);
        movementStateMachine.AddState("fall", fallState);
        movementStateMachine.Init(moveState);

        moveDir = Vector3.forward;
    }


    void Update()
    {
        movementStateMachine.ReadInput();
        movementStateMachine.Update();
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

    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {

    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {

    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        
    }
}