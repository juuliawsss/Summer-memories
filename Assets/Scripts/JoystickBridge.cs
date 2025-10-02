using UnityEngine;

public class JoystickBridge : MonoBehaviour
{
    public Joystick joystick;               // Reference to Floating Joystick
    public PlayerController player;         // Reference to your player

    void Update()
    {
        if (joystick != null && player != null)
        {
            // Send joystick input to PlayerController
            Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
            player.SetJoystickInput(input);
        }
    }
}

