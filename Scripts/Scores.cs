using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{

    public Text currentScore, highScore;
    
    // Start is called before the first frame update
    void Start()
    {
        currentScore.text = "Your Final Score: " + PlayerPrefs.GetInt("CurrentScore");
        highScore.text = "Your High Score: " + PlayerPrefs.GetInt("HighScore");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
