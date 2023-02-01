using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void restart() {
        SceneManager.LoadScene (1);
    }

    public void mainMenu() {
        SceneManager.LoadScene (0);
    }

    public void Exit() {
        Application.Quit();
    }
}
