using UnityEngine;
using UnityEngine.InputSystem;

public class ArcBallCamera : MonoBehaviour
{
    public Transform targetTransform;
    public InputAction mouseLookAction;
    public float horizontalSensitivity = 0.5f;
    public float verticalSensitivity = 0.5f;

    public float offsetDepth;
    public float offsetHeight;
    public float offsetRight;

    private float deltaX = 0.0f;
    private float deltaY = 0.0f;
    [SerializeField]
    private Vector3 forward;
    [SerializeField]
    private Vector3 right;
    [SerializeField]
    private Vector3 up;

    void Start()
    {
        transform.rotation = Quaternion.identity;
        mouseLookAction.Enable();
        forward = Vector3.forward;
        right = Vector3.right;
        up = Vector3.up;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        Vector2 mouseDelta = mouseLookAction.ReadValue<Vector2>();
        deltaX = /*deltaX +*/ (horizontalSensitivity * mouseDelta.x);
        deltaY = /*deltaY +*/ (verticalSensitivity * mouseDelta.y);
        Quaternion rotation = Quaternion.AngleAxis(-deltaY, right) * Quaternion.AngleAxis(deltaX, up);
        /*Quaternion.Euler(-deltaY, deltaX, 0.0f);*/

        forward = (rotation * forward).normalized;
        right = Vector3.Cross(Vector3.up, forward).normalized;
        up = Vector3.up;// Vector3.Cross(forward, right);
        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        //transform.Rotate(right, -deltaY);
        //transform.Rotate(up, deltaX);

        Vector3 cumulativeOffset = -offsetDepth * forward + offsetHeight * up + offsetRight * right;
        transform.position = (targetTransform.position + cumulativeOffset);

        Debug.DrawRay(transform.position, 5.0f * forward, Color.red);
        Debug.DrawRay(transform.position, 5.0f * right, Color.blue);
        Debug.DrawRay(transform.position, 5.0f * up, Color.green);
    }

    private void OnDestroy()
    {
        mouseLookAction.Disable();
    }

    public Vector3 GetFacingDirection()
    {
        return forward;
    }

    public Vector3 GetSideDirection()
    {
        return right;
    }
}
