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
    public float mouseSensitivity = 10f;

    // Joystick support
    [Header("Joystick Support")]
    public Vector2 joystickInput = Vector2.zero; // Set by Joystick script

    // Private variables
    private CharacterController characterController;
    private Vector3 velocity;
    private float xRotation = 0f;
    private float yRotation = 0f; // Add this at the top with other private variables

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

        // Use joystick input if present
        if (joystickInput != Vector2.zero)
        {
            horizontal = joystickInput.x;
            vertical = joystickInput.y;
        }
        // Use new Input System if available
        else if (moveAction != null)
        {
            Vector2 inputVec = moveAction.ReadValue<Vector2>();
            horizontal = inputVec.x;
            vertical = inputVec.y;
        }
        else
        {
            // Fallback to old Input Manager and WASD
            horizontal = 0f;
            vertical = 0f;
            if (Input.GetKey(KeyCode.A)) horizontal -= 1f;
            if (Input.GetKey(KeyCode.D)) horizontal += 1f;
            if (Input.GetKey(KeyCode.W)) vertical += 1f;
            if (Input.GetKey(KeyCode.S)) vertical -= 1f;
        }

        // Use camera's forward and right for movement direction
        Vector3 move = Vector3.zero;
        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f; // Ignore vertical tilt
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
            move = camRight * horizontal + camForward * vertical;
        }
        else
        {
            // Fallback to player transform if camera not assigned
            move = transform.right * horizontal + transform.forward * vertical;
        }
        Debug.Log($"Input: H={horizontal}, V={vertical}, Move={move}");

        // --- Ground check using Raycast and "Ground" tag ---
        bool isGrounded = false;
        RaycastHit hit;
        // Cast a ray down from the center of the character
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 0.3f))
        {
            if (hit.collider.CompareTag("ground"))
            {
                isGrounded = true;
            }
        }

        // Reset vertical velocity if grounded
        if (isGrounded)
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

        // Handle camera rotation (free look)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player horizontally (Y axis)
        yRotation += mouseX;
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        // Rotate camera vertically (X axis)
        if (cameraTransform != null)
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    // Call this from your Joystick script to update movement input
    public void SetJoystickInput(Vector2 input)
    {
        joystickInput = input;
    }
}