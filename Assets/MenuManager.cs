using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SandboxMenuManager.ResetSandboxSettings();
        SceneManager.LoadScene("PlayGame");
    }

    public void OpenSandboxMenu()
    {
        SceneManager.LoadScene("MainMenuSandbox");
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}