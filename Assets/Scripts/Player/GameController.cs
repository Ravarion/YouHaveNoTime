using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public float afterGamePause;
    public GameObject gameOverCanvas;

    // How many scenes are not levels.  These will be at the top of the load order.
    public int scenesNotScored = 1;

    public bool creativeMode = false;
    public bool debugMode = false;

    private GameObject player;
    private MapMaker mapMaker;
    private Text debugText;
    private int totalAlliesSaved;
    private int totalAlliesLost;
    private int totalEnemiesSaved;
    private int totalEnemiesLost;

    private int savedAllies;
    private int fallenAllies;
    private int savedEnemies;
    private int fallenEnemies;

    private float extraTime = 0;

    private static List<string> debugLog = new List<string>();

    public void StartGame()
    {
        SceneManager.LoadScene(scenesNotScored);
    }

    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(afterGamePause);

        // Calculate the level's score
        savedAllies = GameObject.FindGameObjectsWithTag("Ally").Length;
        fallenAllies = GameObject.FindGameObjectsWithTag("FallenAlly").Length;
        savedEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        fallenEnemies = GameObject.FindGameObjectsWithTag("FallenEnemy").Length;
        int score = GetNewLevelScore();

        // Display the gameover screen
        GameObject gameOverCanvasInstance = Instantiate(gameOverCanvas, Vector3.zero, Quaternion.identity);

        Text scoreText = gameOverCanvasInstance.transform.Find("ScoreText").GetComponent<Text>();
        Text highscoreText = gameOverCanvasInstance.transform.Find("HighscoreText").GetComponent<Text>();
        Text savedText = gameOverCanvasInstance.transform.Find("SavedText").GetComponent<Text>();
        Text enemyText = gameOverCanvasInstance.transform.Find("EnemyText").GetComponent<Text>();

        string currentLevel = SceneManager.GetActiveScene().name;
        int oldHighScore = PlayerPrefs.GetInt(currentLevel + " Highscore");

        // If the player has a new highscore, show slightly different text
        if (score > oldHighScore)
        {
            // Save the new highscore
            PlayerPrefs.SetInt(currentLevel + " Highscore", score);
            scoreText.text = "<color=red>New Highscore: " + score.ToString() + "</color>";
            highscoreText.text = "<color=red>Beaten Highscore: " + oldHighScore.ToString() + "</color>";
        }
        else
        {
            scoreText.text = "<color=red>Score: " + score.ToString() + "</color>";
            highscoreText.text = "<color=red>Highscore: " + oldHighScore.ToString() + "</color>";
        }
        
        // Show the player how many allies they saved
        savedText.text = "You have saved " + savedAllies.ToString() + " <color=Green>allies</color> out of " + (savedAllies + fallenAllies).ToString();
        if (savedAllies > PlayerPrefs.GetInt(currentLevel + " SavedAllies"))
        {
            PlayerPrefs.SetInt(currentLevel + " SavedAllies", savedAllies);
        }

        // Save the latest saved allies and saved enemies count
        PlayerPrefs.SetInt(currentLevel + " AllAllies", savedAllies + fallenAllies);
        enemyText.text = "You let " + savedEnemies.ToString() + " out of " + (savedEnemies + fallenEnemies).ToString() + " <color=red>enemies</color> live";
        PlayerPrefs.SetInt(currentLevel + " SavedEnemies", savedEnemies);
        PlayerPrefs.SetInt(currentLevel + " AllEnemies", savedEnemies + fallenEnemies);

        // Setup the gameover buttons
        gameOverCanvasInstance.transform.Find("RetryButton").GetComponent<Button>().onClick.AddListener(LoadCurrentScene);
        gameOverCanvasInstance.transform.Find("HomeButton").GetComponent<Button>().onClick.AddListener(LoadHomeScene);
        gameOverCanvasInstance.transform.Find("NextButton").GetComponent<Button>().onClick.AddListener(LoadNextScene);

        // Don't show next level button unless the player has saved atleast half of their allies, or they've beaten the level before
        if (((float)savedAllies / ((float)savedAllies + (float)fallenAllies)) < 0.5f && PlayerPrefs.GetInt(currentLevel + " Beaten") != 1)
        {
            gameOverCanvasInstance.transform.Find("NextButton").gameObject.SetActive(false);
        }
        else // Save that the level has been beaten
        {
            PlayerPrefs.SetInt(currentLevel + " Beaten", 1);
        }
    } // End ShowGameOverScreen

    public void SetMovementStyle(int moveStyle)
    {
        PlayerPrefs.SetInt("MovementStyle", moveStyle);
        if(FindObjectOfType<MovementController>())
        {
            FindObjectOfType<MovementController>().ChangeMovementStyle(moveStyle);
        }
    }

    public void SetGameMode(int gameMode)
    {
        PlayerPrefs.SetInt("GameMode", gameMode);
    }

    public void DeleteHighscores()
    {
        for(int i = scenesNotScored; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            PlayerPrefs.SetInt(SceneManager.GetSceneByBuildIndex(i).name + " Highscore", 0);
        }
    }

    public void DeleteProgress()
    {
        for (int i = scenesNotScored; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            PlayerPrefs.SetInt(SceneManager.GetSceneByBuildIndex(i).name + " Beaten", 0);
        }
    }

    public void DeleteCustomMaps()
    {

    }

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void GameOver()
    {
        StartCoroutine("ShowGameOverScreen");
    }

    public void LoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextScene()
    {
        if(SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void LoadHomeScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadGameLevel(int index)
    {
        SceneManager.LoadScene(index + scenesNotScored);
    }

    public void SwitchCreativeMode()
    {
        if(creativeMode)
        {
            PlayerPrefs.SetInt("creativeMode", 0);

            mapMaker.SaveMap(SceneManager.GetActiveScene().name + ".txt");
        }
        else
        {
            PlayerPrefs.SetInt("creativeMode", 1);
        }
        LoadCurrentScene();
    }

    public void SendDebugText(string text)
    {
        debugLog.Add(text);
        if(debugLog.Count > 5)
        {
            debugLog.RemoveAt(0);
        }
        if(debugMode)
        {
            debugText.text = "";
            foreach(string log in debugLog)
            {
                debugText.text += log + "\n";
            }
        }
    }

    void Awake()
    {
        if(GameObject.Find("DebugText"))
        {
            debugText = GameObject.Find("DebugText").GetComponent<Text>();
        }
    }

    void Start()
    {
        creativeMode = PlayerPrefs.GetInt("creativeMode") > 0;
        mapMaker = FindObjectOfType<MapMaker>();
        if(creativeMode)
        {
            TimeTracker timeTracker = GetComponent<TimeTracker>();
            if(timeTracker)
            {
                timeTracker.SetTimeScale(0);
            }
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.L))
        {
            if(debugText.color != Color.red)
            {
                debugText.color = Color.red;
            }
            else
            {
                debugText.color = Color.clear;
            }
        }

        if(Input.GetButtonUp("Time Restore/Spawn"))
        {
            if(!creativeMode)
            {
                if (extraTime == 0)
                {
                    extraTime = FindObjectOfType<TimeTracker>().timeLeft;
                    FindObjectOfType<TimeTracker>().timeLeft = -1;
                }
            }
        }

        if(Input.GetButtonUp("SwitchMovementStyle"))
        {
            if (PlayerPrefs.GetInt("MovementStyle") == (int)MovementController.MovementStyle.CHARACTER_ORIENT)
            {
                SetMovementStyle((int)MovementController.MovementStyle.MAP_ORIENT);
            }
            else
            {
                SetMovementStyle((int)MovementController.MovementStyle.CHARACTER_ORIENT);
            }
        }

        if(Input.GetButtonUp("SwitchCreativeMode"))
        {
            SwitchCreativeMode();
        }
    }

    private int GetNewLevelScore()
    {
        float savedAlliesRatio = savedAllies / ((float)savedAllies + fallenAllies);
        // First 500 points are for how many allies you saved
        int score = (int)(500f * savedAlliesRatio);
        // The second 500 points are for how many allies you saved and how quickly you saved them
        score += (int)(500f * extraTime * savedAlliesRatio);
        return score;
    }
}
