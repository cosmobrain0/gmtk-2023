using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIController : MonoBehaviour
{
    public void retryLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void loadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
