using UnityEngine;

// Fixed typo in class name: Joysick -> Joystick
public class Joystick : MonoBehaviour
{
    public Transform player; // Assign your player GameObject's Transform in the Inspector
    public float moveSpeed = 5f;

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private Vector2 direction;
    private bool isTouching = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
                    MovePlayer(direction);
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
                MovePlayer(direction);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isTouching = false;
                direction = Vector2.zero;
            }
        }
    }

    void MovePlayer(Vector2 dir)
    {
        if (player != null && dir != Vector2.zero)
        {
            Vector3 move = new Vector3(dir.x, 0, dir.y) * moveSpeed * Time.deltaTime;
            player.position += move;
        }
    }
}
