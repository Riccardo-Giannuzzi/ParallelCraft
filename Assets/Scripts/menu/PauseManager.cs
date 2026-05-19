using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  Manages the pause functionality of the game, allowing the player to return to the main menu when the Escape key is pressed.
/// </summary>
public class PauseManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("menu");
        }
    }
}