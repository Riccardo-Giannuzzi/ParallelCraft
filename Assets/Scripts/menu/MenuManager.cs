using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the main menu, providing functionality to start the game or exit the application when the corresponding buttons are pressed.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}