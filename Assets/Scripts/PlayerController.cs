using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Input System variables
    private PlayerInput playerInput;
    private InputAction moveAction;
    // Movement variables
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    // Camera variables
    public Transform cameraTransform;
    public float mouseSensitivity = 100f;

    // Private variables
    private CharacterController characterController;
    private Vector3 velocity;
    private float xRotation = 0f;

    void Start()
    {
        // Setup new Input System if available
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
        }

        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();

        // Warn if cameraTransform is not assigned
        if (cameraTransform == null)
        {
            Debug.LogWarning("PlayerController: cameraTransform is not assigned! Please assign your camera in the Inspector.");
        }

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Handle movement
        float horizontal = 0f;
        float vertical = 0f;

        // Use new Input System if available
        if (moveAction != null)
        {
            Vector2 inputVec = moveAction.ReadValue<Vector2>();
            horizontal = inputVec.x;
            vertical = inputVec.y;
        }
        else
        {
            // Fallback to old Input Manager and WASD
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            if (Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f))
            {
                if (Input.GetKey(KeyCode.A)) horizontal = -1f;
                if (Input.GetKey(KeyCode.D)) horizontal = 1f;
                if (Input.GetKey(KeyCode.W)) vertical = 1f;
                if (Input.GetKey(KeyCode.S)) vertical = -1f;
            }
        }

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        Debug.Log($"Input: H={horizontal}, V={vertical}, Move={move}");

        // Ground check and reset vertical velocity if grounded
        if (characterController.isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f; // Small negative to keep grounded
            }
            // Handle jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = move * moveSpeed;
        finalMove.y = velocity.y;
        characterController.Move(finalMove * Time.deltaTime);

        // Prevent falling under the ground (y < 0)
        if (transform.position.y < 0f)
        {
            Vector3 pos = transform.position;
            pos.y = 0f;
            transform.position = pos;
            velocity.y = 0f;
        }

        // Handle camera rotation (free look)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (cameraTransform != null)
        {
            // Accumulate rotation
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Get current y rotation
            float yRotation = cameraTransform.localEulerAngles.y + mouseX;
            // If yRotation goes above 360 or below 0, keep it in [0,360)
            if (yRotation > 360f) yRotation -= 360f;
            if (yRotation < 0f) yRotation += 360f;

            cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
        else
        {
            // Optionally, warn every frame if not assigned (comment out if too spammy)
            // Debug.LogWarning("PlayerController: cameraTransform is not assigned!");
        }
    }
}