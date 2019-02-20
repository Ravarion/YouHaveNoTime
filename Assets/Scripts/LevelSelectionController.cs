using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionController : MonoBehaviour {

    public GameObject[] levelButtons;

    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();

        // First level will always be available.
        levelButtons[0].SetActive(true);

        Text allyText = levelButtons[0].transform.Find("ScoreBG").transform.Find("FriendlyText").GetComponent<Text>();
        Text enemyText = levelButtons[0].transform.Find("ScoreBG").transform.Find("EnemyText").GetComponent<Text>();

        string levelName = SceneUtility.GetScenePathByBuildIndex(gameController.scenesNotScored);
        levelName = levelName.Split('/')[levelName.Split('/').Length - 1].Split('.')[0];

        allyText.text = "(" + PlayerPrefs.GetInt(levelName + " SavedAllies").ToString() + "/" + PlayerPrefs.GetInt(levelName + " AllAllies").ToString() + ")";
        enemyText.text = "(" + PlayerPrefs.GetInt(levelName + " SavedEnemies").ToString() + "/" + PlayerPrefs.GetInt(levelName + " AllEnemies").ToString() + ")";

        int scenesNotScored = FindObjectOfType<GameController>().scenesNotScored;

        // For each level in the game
        for (int i = scenesNotScored; i < levelButtons.Length; i++)
        {
            levelName = SceneUtility.GetScenePathByBuildIndex(i + 1);
            levelName = levelName.Split('/')[levelName.Split('/').Length - 1].Split('.')[0];

            levelButtons[i].SetActive(false);

            if (PlayerPrefs.GetInt(levelName + " Beaten") > 0)
            {
                levelButtons[i].SetActive(true);

                string localLevelName = levelName;
                levelButtons[i].GetComponent<Button>().onClick.AddListener(() => gameController.LoadLevel(localLevelName));

                allyText = levelButtons[i].transform.Find("ScoreBG").transform.Find("FriendlyText").GetComponent<Text>();
                enemyText = levelButtons[i].transform.Find("ScoreBG").transform.Find("EnemyText").GetComponent<Text>();

                allyText.text = "(" + PlayerPrefs.GetInt(levelName + " SavedAllies").ToString() + "/" +
                    PlayerPrefs.GetInt(levelName + " AllAllies").ToString() + ")";
                enemyText.text = "(" + PlayerPrefs.GetInt(levelName + " SavedEnemies").ToString() + "/" +
                    PlayerPrefs.GetInt(levelName + " AllEnemies").ToString() + ")";
            }
        }
    }

    
}
