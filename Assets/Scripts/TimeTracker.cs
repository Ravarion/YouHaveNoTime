using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeTracker : MonoBehaviour {

    public float timeLeft;
    public Text timeText;
    public float timeScale;
    public bool superHotMode = false;

    public enum GameMode
    {
        QUICKSILVER,
        SUPER_HOT
    }

    void Start()
    {
        superHotMode = PlayerPrefs.GetInt("GameMode") == (int)GameMode.SUPER_HOT;
        // Give extra time if playing in superHotMode
        if (superHotMode)
        {
            timeScale /= 30;
        }

        Time.timeScale = timeScale;
    }

    void Update()
    {
        if(timeLeft < 0)
        {
            timeLeft = 0;
            Time.timeScale = 1;
            FindObjectOfType<GameController>().GameOver();
        }
        else if(timeLeft == 0)
        {
            if (timeText != null)
            {
                timeText.text = "You Have <color=red>No</color> Time";
            }
        }
        else
        {
            if (timeText != null)
            {
                timeText.text = "You Have <color=red>" + (timeLeft).ToString("F3") + "</color> Seconds!";
            }
            timeLeft -= Time.deltaTime;
        }
    }

    public void SetTimeScale(int val)
    {
        timeScale = val;
        Time.timeScale = timeScale;
    }
}
