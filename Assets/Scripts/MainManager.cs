using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public GameObject playBtn;
    public GameObject startScreen;
    public string username;
    public string champion;
    public int toZero = 0;    

    public Text ScoreText;
    public Text maxScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private static int maxScore = 0;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {   
        Time.timeScale = 0;
        playBtn = GameObject.FindGameObjectWithTag("PlayBtn");
        startScreen = GameObject.FindGameObjectWithTag("StartScreen");
        maxScore = PlayerPrefs.GetInt("MaxScore");
        username = PlayerPrefs.GetString("PName");
        champion = PlayerPrefs.GetString("Champion");
        Debug.Log("Current Max score: " + maxScore);
        Debug.Log("Current Player: " + username);
        Debug.Log("Current Champion: " + champion);
        maxScoreText.text = $"[ Highest Score ]\n{champion} - {maxScore}";
        //PlayerPrefs.SetInt("MaxScore", maxScore);

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        startScreen.gameObject.SetActive(false);
    }

    public void ResScore() {
        PlayerPrefs.SetInt("MaxScore", toZero);
        maxScore = PlayerPrefs.GetInt("MaxScore");
        maxScoreText.text = $"[ Highest Score ]\n{champion} - {maxScore}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (m_Points > maxScore) {
            maxScore = m_Points;
            PlayerPrefs.SetInt("MaxScore", maxScore);
            PlayerPrefs.SetString("Champion", username);
            champion = PlayerPrefs.GetString("Champion");
            maxScoreText.text = $"[ Highest Score ]\n{champion} - {maxScore}";
        }
        Debug.Log("Max score: " + maxScore);
    }
}
