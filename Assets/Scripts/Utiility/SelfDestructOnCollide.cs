using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructOnCollide : MonoBehaviour {

    private void OnCollisionStay(Collision collision)
    {
        if(!collision.gameObject.GetComponent<Rigidbody>())
        {
            Destroy(gameObject);
        }
    }
}
