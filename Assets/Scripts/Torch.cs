using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Torch : MonoBehaviour {
	
	void Update ()
    {
        foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, .5f, Vector3.forward, .5f))
        {
            if (hit.transform.gameObject.GetComponent<ITorchable>() != null)
            {
                hit.transform.gameObject.GetComponent<ITorchable>().Torch();
            }
        }
    }
}
