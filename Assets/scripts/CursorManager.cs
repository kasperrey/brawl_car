using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private bool shouldBeLocked = true;

    private void Awake()
    {
        // Lock cursor immediately when the script loads
        shouldBeLocked = true;
        MainMenuManager.SetCursorState(true);
    }

    private void Start()
    {
        // Ensure cursor is locked after scene is fully loaded
        shouldBeLocked = true;
        MainMenuManager.SetCursorState(true);
    }

    private void Update()
    {
        // Optional: Add escape key to toggle cursor lock
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            shouldBeLocked = !shouldBeLocked;
            MainMenuManager.SetCursorState(shouldBeLocked);
        }

        // Only update cursor state if it doesn't match our desired state
        if (shouldBeLocked && Cursor.lockState != CursorLockMode.Locked)
        {
            MainMenuManager.SetCursorState(true);
        }
        else if (!shouldBeLocked && Cursor.lockState == CursorLockMode.Locked)
        {
            MainMenuManager.SetCursorState(false);
        }
    }
} 