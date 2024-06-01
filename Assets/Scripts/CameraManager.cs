using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject lookAt;
    public Transform cameraPivot; // The object the camera uses to look up & down (pivot)
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 1.0f;
    public float cameraPivotSpeed = 1.0f;
    public float minimumPivotAngle = -20.0f;
    public float maximumPivotAngle = 60.0f;
    public float minimumCollisionOffset = 0.2f;
    public float cameraCollisionRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f; // How much the camera will jump off of objects its colliding with
    public LayerMask collisionLayers;
    public float lookAngle; // Camera looking up & down
    public float pivotAngle; // Camera looking left & right

    private float defaultPosition;
    private Transform mainCameraTransform;
    private Transform targetTransform; // The object that the camera will follow
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;
    private InputManager im;

    private void Awake()
    {
        targetTransform = lookAt.transform;
        im = FindObjectOfType<InputManager>();
        mainCameraTransform = Camera.main.transform;
        defaultPosition = mainCameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle += im.cameraInput.x * cameraLookSpeed;
        pivotAngle -= im.cameraInput.y * cameraPivotSpeed;
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        // Y Axis
        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        // X Axis
        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = mainCameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition -= minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(mainCameraTransform.localPosition.z, targetPosition, 0.2f);
        mainCameraTransform.localPosition = cameraVectorPosition;
    }
}