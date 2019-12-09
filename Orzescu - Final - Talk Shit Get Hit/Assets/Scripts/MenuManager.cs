using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {








    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
