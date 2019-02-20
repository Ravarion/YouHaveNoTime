using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject toFollow;
    public Vector3 distance;
    public Vector3 rotation;

    void Start()
    {
        transform.eulerAngles = rotation;
    }


    void Update()
    {
        if (toFollow == null)
        {
            MovementController playerScript = FindObjectOfType<MovementController>();
            if(playerScript != null)
            {
                toFollow = playerScript.gameObject;
            }
        }
        else
        {
            transform.position = toFollow.transform.position + distance;
        }
    }
}
