using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMove : MonoBehaviour {

    public Vector3 direction;
    public Vector3 rotation;
    public bool scaleByTime = true;
    private TimeTracker timeTracker;

    void Start()
    {
        timeTracker = FindObjectOfType<TimeTracker>();
    }


    void Update()
    {
        Vector3 toTranslate = direction;
        Vector3 toRotate = rotation;
        if (scaleByTime)
        {
            toTranslate *= timeTracker.timeScale;
            toRotate *= timeTracker.timeScale;
        }
        transform.Translate(toTranslate);
        transform.Rotate(toRotate);
    }
}
