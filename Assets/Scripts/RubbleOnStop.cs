using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleOnStop : MonoBehaviour {

    private bool wasAwake = false;

	void FixedUpdate ()
    {
		if(GetComponent<Rigidbody>().IsSleeping())
        {
            if (wasAwake)
            {
                GetComponent<Collider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else
        {
            wasAwake = true;
        }
	}
}
