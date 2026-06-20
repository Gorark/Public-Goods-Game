using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OpenSandboxMenu()
    {
        SceneManager.LoadScene("MainMenuSandbox");
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
