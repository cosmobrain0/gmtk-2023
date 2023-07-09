using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenuUIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadGame()
    {
        //Change with index of scene in build settings
        SceneManager.LoadScene(1);
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
