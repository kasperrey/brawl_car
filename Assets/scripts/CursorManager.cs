using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private void Awake()
    {
        // Lock cursor immediately when the script loads
        MainMenuManager.SetCursorState(true);
    }

    private void Start()
    {
        // Ensure cursor is locked after scene is fully loaded
        MainMenuManager.SetCursorState(true);
    }

    private void Update()
    {
        // Optional: Add escape key to toggle cursor lock
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isCurrentlyLocked = Cursor.lockState == CursorLockMode.Locked;
            MainMenuManager.SetCursorState(!isCurrentlyLocked);
        }

        // Force cursor to stay locked if it should be
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            MainMenuManager.SetCursorState(true);
        }
    }
} 