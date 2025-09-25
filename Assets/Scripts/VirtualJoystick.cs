using UnityEngine;

public class VirtualJoystick : MonoBehaviour
{
    // Reference to PlayerController to send joystick input
    public PlayerController playerController;

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private Vector2 direction;
    private bool isTouching = false;

    void Update()
    {
        direction = Vector2.zero;

        // For mobile touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isTouching = true;
                    break;
                case TouchPhase.Moved:
                    currentTouchPosition = touch.position;
                    direction = (currentTouchPosition - startTouchPosition).normalized;
                    break;
                case TouchPhase.Ended:
                    isTouching = false;
                    direction = Vector2.zero;
                    break;
            }
        }
        // For testing in editor with mouse
        else if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouchPosition = Input.mousePosition;
                isTouching = true;
            }
            else if (Input.GetMouseButton(0) && isTouching)
            {
                currentTouchPosition = Input.mousePosition;
                direction = ((Vector2)currentTouchPosition - startTouchPosition).normalized;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isTouching = false;
                direction = Vector2.zero;
            }
        }

        // Send joystick direction to PlayerController
        if (playerController != null)
        {
            playerController.SetJoystickInput(direction);
        }
    }
}