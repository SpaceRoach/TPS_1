using UnityEngine;

public class LocomotionFSM : StateControllerBase
{
    public float groundMoveModifier = 10.0f;
    public float airMoveModifier;
    public Vector3 playerHVel;
    public Vector3 moveDir;
}
