using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Speeds")]
    //[SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float cameraSensitivity = 15.0f;

    //[Header("Monitor speed (do not edit)")]
    //[SerializeField] private float currentSpeed = 6.0f;

    private InputManager im;
    private CameraManager cm;

    private Transform mainCameraTransform;
    private Vector3 movementDirection;
    private Rigidbody characterRigidbody;


    public bool phase2;

    public static PlayerMovementController Instance; //singleton to make class referenceable from anything
    
    private bool grounded;
    private void Awake()
    {
        Instance = this;
        mainCameraTransform = Camera.main.transform;
        characterRigidbody = GetComponent<Rigidbody>();
        im = FindObjectOfType<InputManager>();
        cm = FindObjectOfType<CameraManager>();

    }

    // To move the actual character using RigidBody (physics)
    private void FixedUpdate()
    {
        HandleAllMovement();
        phase2 = Boss.Instance.phase2;
    }

    private void LateUpdate()
    {
        if (Time.timeScale != 0.0f && phase2) cm.HandleAllCameraMovement();
    }

    // Player Input Handling

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (Health.Instance.health <= 0) return; //cant move if health is less than 0
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            characterRigidbody.AddForce(Vector3.up * 360f);
        }


        if (phase2)
        {
            movementDirection = new Vector3(mainCameraTransform.forward.x, 0, mainCameraTransform.forward.z) * im.movementInput.y;
        }
        else
        {
            movementDirection = new Vector3(0, 0, mainCameraTransform.forward.z) * im.movementInput.y;
        }
        movementDirection += mainCameraTransform.right * im.movementInput.x;

        movementDirection.Normalize();
        movementDirection.y = 0;

        movementDirection *= runSpeed;

        Vector3 movementVelocity = movementDirection;
        movementVelocity.y = characterRigidbody.velocity.y;
        characterRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

            targetDirection = mainCameraTransform.forward * im.movementInput.y;
            targetDirection += mainCameraTransform.right * im.movementInput.x;
        
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraSensitivity * Time.deltaTime);

        transform.rotation = playerRotation;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            grounded = false;
        }
    }
    // Extra function to return run speed (function might expand in future to include walk or sprint modes if needed)
    //private float GetPlayerSpeed()
    //{
    //    switch (im.animationSelector)
    //    {
    //        case 3:
    //            currentSpeed = dashSpeed; break;
    //        case 2:
    //            currentSpeed = runSpeed; break;
    //        case 1:
    //            currentSpeed = walkSpeed; break;
    //        default:
    //            currentSpeed = 0; break;
    //    }
    //
    //    // Debug.Log($"Setting speed to: {currentSpeed}...");
    //    return currentSpeed;
    //}
}