using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasManager : MonoBehaviour
{
    public Canvas mainCanvas; // For your main UI elements
    public Canvas joystickCanvas; // For the joystick UI
    public Toggle joystickToggle; // Reference to the UI Toggle
    
    private void Start()
    {
        // Ensure Event System exists
        if (FindAnyObjectByType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
        
        // Ensure both canvases are active
        if (mainCanvas != null)
            mainCanvas.gameObject.SetActive(true);
            
        // Set up the toggle listener
        if (joystickToggle != null)
        {
            joystickToggle.onValueChanged.AddListener(ShowJoystickCanvas);
            // Set initial state
            ShowJoystickCanvas(joystickToggle.isOn);
        }
    }

    public void ShowJoystickCanvas(bool show)
    {
        if (joystickCanvas != null)
            joystickCanvas.gameObject.SetActive(show);
    }
} 