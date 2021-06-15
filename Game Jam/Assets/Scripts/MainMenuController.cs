using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject quitButton;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("'Quit' button clicked. ");
        Application.Quit();
    }

    public void OpenOptions()
    {
        Debug.Log("'Options' button clicked. ");

    }
}
