using UnityEngine;

public class CarInputs : MonoBehaviour
{
    public Vector2 movement = Vector2.zero;
    public bool isJoystickEnabled = false;
    
    private void FixedUpdate()
    { 
        if (!isJoystickEnabled)
        {
            // Get keyboard input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            // Only update if there's actual input
            if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
            {
                movement = new Vector2(horizontal, vertical);
                isJoystickEnabled = false;
            }
        }
    }
    
    public void GetJoystickData(Vector2 joystickMovement)
    {
        // Only update if there's actual joystick movement
        if (joystickMovement.magnitude > 0.1f)
        {
            movement = joystickMovement;
            isJoystickEnabled = true;
        }
        else
        {
            // Reset movement when joystick input is very small
            movement = Vector2.zero;
            isJoystickEnabled = false;
        }
    }
}
