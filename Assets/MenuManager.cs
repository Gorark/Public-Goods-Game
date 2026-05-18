using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OpenGame()
    {
        SceneManager.LoadScene("PlayGame");
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
