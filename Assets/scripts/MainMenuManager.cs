using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    
    private void Start()
    {
        // Make sure cursor is visible and unlocked in main menu
        SetCursorState(false);
    }

    public void StartGame() {
        string selectedScene = dropdown.options[dropdown.value].text;
        SceneManager.LoadScene(selectedScene);
        
        // Lock cursor when game starts
        SetCursorState(true);
    }

    public static void SetCursorState(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
