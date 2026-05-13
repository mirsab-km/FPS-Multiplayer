using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenButtons : MonoBehaviourPunCallbacks
{
    public GameObject gameOverScreen;
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        gameOverScreen.SetActive(false);
        AudioListener.pause = false;

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        PhotonNetwork.LeaveRoom();

        SceneManager.LoadScene("MainMenu");
    }
}