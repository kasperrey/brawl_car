using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButton : MonoBehaviour
{
    public void OpenMenu() {
        Debug.Log("Opening menu");
        // Unlock cursor before loading menu
        MainMenuManager.SetCursorState(false);
        SceneManager.LoadScene("MainMenu");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Menu button pressed");
            OpenMenu();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C button pressed");
        }
    }
}
