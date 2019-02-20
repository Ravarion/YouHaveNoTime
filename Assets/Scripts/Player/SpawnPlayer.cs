using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPlayer : MonoBehaviour {

    public GameObject playerPrefab;

    void Start()
    {
        // If the player isn't spawned yet
        if(!FindObjectOfType<MovementController>() && SceneManager.GetActiveScene().buildIndex != 0)
        {
            GameObject newSpawn = Instantiate(playerPrefab, transform.position, transform.rotation);
            newSpawn.transform.SetParent(GameObject.Find("Map").transform);

            // If we're not in creative mode, self destruct
            if (!FindObjectOfType<GameController>().creativeMode)
            {
                Destroy(gameObject);
            }
        }
        // If we're in creative mode, destroy any other spawners.  There can only be one!
        if(FindObjectOfType<GameController>().creativeMode)
        {
            SpawnPlayer[] scripts = FindObjectsOfType<SpawnPlayer>();
            if(scripts.Length > 1)
            {
                for(int i = 0; i < scripts.Length; i++)
                {
                    if(scripts[i] != this)
                    {
                        Destroy(scripts[i].gameObject);
                    }
                }
            }
        }
    }
}
