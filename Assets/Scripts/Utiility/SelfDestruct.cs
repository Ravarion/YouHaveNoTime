using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {

    public float timeDelay;
    public bool scaledTime;

    void Start()
    {
        if(scaledTime)
        {
            timeDelay -= Time.deltaTime;
        }
        else
        {
            timeDelay -= Time.unscaledDeltaTime;
        }
        if(timeDelay <= 0)
        {
            Destroy(gameObject);
        }
    }
}
