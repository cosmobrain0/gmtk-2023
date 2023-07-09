using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUIController : MonoBehaviour
{
    public void loadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
