using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OpenGame()
    {
        SceneManager.LoadScene("GameRoom");
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
