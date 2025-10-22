using UnityEngine;

public class JoystickBridge : MonoBehaviour
{
    // This script is no longer needed since PlayerController now reads directly from the joystick
    // You can delete this script and assign the joystick directly to PlayerController.fixedJoystick in the Inspector
    
    /*
    Alternative: If you want to keep this bridge pattern, you could do:
    
    public Joystick joystick;
    public PlayerController player;

    void Update()
    {
        if (joystick != null && player != null)
        {
            // Set the joystick reference if not already set
            if (player.fixedJoystick == null && joystick is FixedJoystick)
            {
                player.fixedJoystick = joystick as FixedJoystick;
            }
        }
    }
    */
}

