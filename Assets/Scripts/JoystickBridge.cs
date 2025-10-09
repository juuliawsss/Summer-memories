using UnityEngine;

public class JoystickBridge : MonoBehaviour
{
    public Joystick joystick;
    public PlayerController player;

    void Update()
    {
        if (joystick != null && player != null)
        {
            Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
            player.SetJoystickInput(input);

            // 🔍 Add this line:
            Debug.Log($"[JoystickBridge] Input: {input}");
        }
    }
}

