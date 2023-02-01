using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rb;
    public int jumpForce = 275, horizontalSpeed = 3, pipeDistance = 10;
    bool start = false, ended = false;
    public GameObject pipe1, pipe2, pipe3, boundaries;
    public GameObject [] clouds = new GameObject[4];
    public AudioSource ping, oof;

    int pipeToMove = 1, score = 0;

    public Text counter, directions;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        spawnPipes();
        spawnClouds();
        counter.text = "Score: " + score;
        PlayerPrefs.SetInt("CurrentScore", 0);

    }

    // Update is called once per frame
    void Update()
    {
    
        if(start != false && ended == false) {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) {
                jump();
            }
            setHorizontalSpeed();
        } else if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && ended == false){
            startGame();
        }

        movePipes();
        moveClouds();

    }

    void updateScore() {
        score++;
        counter.text = "Score: " + score;
        PlayerPrefs.SetInt("CurrentScore", score);
        ping.Play();
    }
    
    void startGame() {
        rb.constraints = RigidbodyConstraints2D.None;
        setHorizontalSpeed();
        jump();
        start = true;
        directions.enabled = false;
    }
    
    void setHorizontalSpeed(){
        rb.velocity = new Vector2(horizontalSpeed, rb.velocity.y);
        for(int i = 0; i<clouds.Length; i++) {
            //clouds[i].velocity = new Vector2(cloudSpeed, 0);
            clouds[i].transform.position = new Vector2(clouds[i].transform.position.x - 0.001f, clouds[i].transform.position.y);
        }
    }
    
    void jump() {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, jumpForce));
    }
    
    void spawnPipes() {
        pipe1 = Instantiate(pipe1, new Vector2(transform.position.x+pipeDistance, Random.Range(-.5f, 3.5f)), Quaternion.identity);
        pipe2 = Instantiate(pipe2, new Vector2(transform.position.x+pipeDistance*2, Random.Range(-.5f, 3.5f)), Quaternion.identity);
        pipe3 = Instantiate(pipe3, new Vector2(transform.position.x+pipeDistance*3, Random.Range(-.5f, 3.5f)), Quaternion.identity);
    }

    void spawnClouds() {
        clouds[0] = Instantiate(clouds[0], new Vector2(Random.Range(-9f, -2.5f), Random.Range(-3, 4.9f)), Quaternion.identity);
        clouds[1] = Instantiate(clouds[1], new Vector2(Random.Range(6f, 12.5f), Random.Range(-3, 4.9f)), Quaternion.identity);
        clouds[2] = Instantiate(clouds[2], new Vector2(Random.Range(-1.5f, 5f), Random.Range(-3, 4.9f)), Quaternion.identity);
        clouds[3] = Instantiate(clouds[3], new Vector2(Random.Range(13.5f, 20f), Random.Range(-3, 4.9f)), Quaternion.identity);
    }

    void moveClouds() {
        for(int i = 0; i<clouds.Length; i++) {
            if(clouds[i].transform.position.x < rb.transform.position.x-10) {
                clouds[i].transform.position = new Vector2(rb.transform.position.x + 2 * (rb.transform.position.x-clouds[i].transform.position.x), Random.Range(-3f, 4.9f));
            }
        }
    }

    void movePipes() {
        if(rb.transform.position.x > pipe1.transform.position.x && score == 0){
            updateScore();
        }

        if(pipeToMove == 1 && rb.transform.position.x > pipe2.transform.position.x) {
            pipe1.transform.position = new Vector2(pipe3.transform.position.x+pipeDistance, Random.Range(-.5f, 3.5f));
            pipeToMove = 2;
            updateScore();
        } else if(pipeToMove == 2 && rb.transform.position.x > pipe3.transform.position.x) {
            pipe2.transform.position = new Vector2(pipe1.transform.position.x+pipeDistance, Random.Range(-.5f, 3.5f));
            pipeToMove = 3;
            updateScore();
        } else if(pipeToMove == 3 && rb.transform.position.x > pipe1.transform.position.x) {
            pipe3.transform.position = new Vector2(pipe2.transform.position.x+pipeDistance, Random.Range(-.5f, 3.5f));
            pipeToMove = 1;
            boundaries.transform.position = new Vector2(pipe3.transform.position.x, boundaries.transform.position.y);
            updateScore();
        }
    }
    
    void OnTriggerEnter2D(Collider2D col) {
        oof.Play();
        setHighScore(score);
        ended = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        StartCoroutine(EndAfterTime(.5f));
    }

    void setHighScore(int newScore) {
        if(newScore > PlayerPrefs.GetInt("HighScore")) {
           PlayerPrefs.SetInt("HighScore", score); 
        }
    }

    IEnumerator EndAfterTime(float time) {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(2);
    }

}
