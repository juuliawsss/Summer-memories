using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
        // Get the CharacterController component
        characterController = GetComponent<CharacterController>();

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Handle movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Only apply horizontal movement to move vector
        characterController.Move(move * moveSpeed * Time.deltaTime);

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
        characterController.Move(velocity * Time.deltaTime);

        // Handle camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}