using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Input System variables
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    // Movement variables
    public float moveSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    // Camera variables
    public Transform cameraTransform;
    public float mouseSensitivity = 10f;

    // Add this reference to your joystick
    public FixedJoystick joystick_move;
    public FixedJoystick joystick_camera;

    // Private variables
    private CharacterController characterController;
    private Vector3 velocity;
    private float xRotation = 0f; // Camera vertical rotation
    private float yRotation = 0f; // Player horizontal rotation

    void Start()
    {
        // Setup new Input System if available
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
            lookAction = playerInput.actions["Look"];
        }

        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();

        // Warn if cameraTransform is not assigned
        if (cameraTransform == null)
        {
            Debug.LogWarning("PlayerController: cameraTransform is not assigned! Please assign your camera in the Inspector.");
        }

        // Lock cursor for mouse look (press Escape to unlock)
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Handle movement
        float horizontal = 0f;
        float vertical = 0f;

        // Get input from joystick if assigned
        if (joystick_move != null)
        {
            horizontal = joystick_move.Horizontal;
            vertical = joystick_move.Vertical;
            
            // Debug to see if joystick is working
            if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
            {
                Debug.Log($"Movement Joystick Input: X={horizontal:F2}, Y={vertical:F2}");
            }
        }
        // Fallback to Input System if no joystick input
        else if (moveAction != null)
        {
            Vector2 inputVec = moveAction.ReadValue<Vector2>();
            horizontal = inputVec.x;
            vertical = inputVec.y;
        }
        // Keyboard fallback
        else if (UnityEngine.InputSystem.Keyboard.current != null)
        {
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard.wKey.isPressed) vertical = 1f;
            if (keyboard.sKey.isPressed) vertical = -1f;
            if (keyboard.aKey.isPressed) horizontal = -1f;
            if (keyboard.dKey.isPressed) horizontal = 1f;
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
            // Handle jumping with Input System
            if (jumpAction != null && jumpAction.triggered)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Combine horizontal and vertical movement
        Vector3 finalMove = move * moveSpeed;
        finalMove.y = velocity.y;
        
        // Debug the final movement
        if (horizontal != 0 || vertical != 0)
        {
            Debug.Log($"Final Move: {finalMove}, Speed: {moveSpeed}, H={horizontal:F2}, V={vertical:F2}");
        }
        
        characterController.Move(finalMove * Time.deltaTime);

        // Handle camera rotation with joystick or mouse
        if (cameraTransform != null)
        {
            float mouseX = 0f;
            float mouseY = 0f;
            
            // Get input from camera joystick if assigned
            if (joystick_camera != null)
            {
                mouseX = joystick_camera.Horizontal * mouseSensitivity * 0.5f; // Lower multiplier for smoother control
                mouseY = joystick_camera.Vertical * mouseSensitivity * 0.5f;

                // Optionally invert Y axis for typical camera control
                mouseY = -mouseY;

                // Debug camera joystick input
                if (Mathf.Abs(joystick_camera.Horizontal) > 0.1f || Mathf.Abs(joystick_camera.Vertical) > 0.1f)
                {
                    Debug.Log($"Camera Joystick Input: X={joystick_camera.Horizontal:F2}, Y={joystick_camera.Vertical:F2}");
                }
            }
            // Fallback to Input System for mouse
            else if (lookAction != null)
            {
                Vector2 lookInput = lookAction.ReadValue<Vector2>();
                mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
                mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
            }
            // Fallback to Mouse class for mouse input
            else if (UnityEngine.InputSystem.Mouse.current != null)
            {
                var mouse = UnityEngine.InputSystem.Mouse.current;
                Vector2 mouseDelta = mouse.delta.ReadValue();
                mouseX = mouseDelta.x * mouseSensitivity * 0.01f;
                mouseY = mouseDelta.y * mouseSensitivity * 0.01f;
            }
            
            // Apply rotation
            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            
            // Rotate player horizontally (Y axis)
            transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
            
            // Rotate camera vertically (X axis)
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}