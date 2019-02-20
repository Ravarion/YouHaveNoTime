using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour {

    public GameObject[] toSpawn;
    public float waitForRate;
    public int maxSpawned;

    private List<GameObject> spawned;

	void Start()
    {
        spawned = new List<GameObject>();
        StartCoroutine("Spawn");
	}

    private void Update()
    {
        if(spawned.Count > maxSpawned)
        {
            Destroy(spawned[0]);
            spawned.RemoveAt(0);
        }
    }

    IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(waitForRate);
            spawned.Add(Instantiate(toSpawn[Random.Range(0, toSpawn.Length)], transform.position, Quaternion.Euler(Random.Range(-360f,360f), Random.Range(-360f, 360f), Random.Range(-360f, 360f))));
        }
    }
}
