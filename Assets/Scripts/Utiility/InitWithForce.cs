using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitWithForce : MonoBehaviour
{

    public Vector3 force;
    public float forceScalar;

    void Start()
    {
        if(force == Vector3.zero)
        {
            force = new Vector3(Random.Range(-1f, 1f) * forceScalar, Random.Range(-1f, 1f) * forceScalar, Random.Range(-1f, 1f) * forceScalar);
        }
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}
