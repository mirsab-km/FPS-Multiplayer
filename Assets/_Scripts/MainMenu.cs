using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MAP 1");
    }

    public void Exit()
    {
        Application.Quit();
    }


}
