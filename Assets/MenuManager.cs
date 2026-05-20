using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayGame");
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
