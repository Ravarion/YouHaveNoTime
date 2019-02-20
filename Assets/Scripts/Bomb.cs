using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public float countdownTime;
    public float damageDistance;
    public GameObject explosionEffect;
	
	void Update()
    {
        countdownTime -= Time.deltaTime;
        if(countdownTime < 0)
        {
            Explode();
        }
	}

    void Explode()
    {
        foreach(RaycastHit hit in Physics.SphereCastAll(transform.position, damageDistance, Vector3.forward))
        {
            if (hit.transform.gameObject.GetComponent<IBreakable>() != null)
            {
                RaycastHit rayHit;
                transform.LookAt(hit.transform.position);
                if(Physics.Raycast(transform.position, transform.forward, out rayHit))
                {
                    if(rayHit.transform.gameObject == hit.transform.gameObject)
                    {
                        hit.transform.gameObject.GetComponent<IBreakable>().Break(damageTypes.bomb);
                    }
                }
            }
        }

        foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, damageDistance, Vector3.forward))
        {
            if (hit.transform.GetComponent<Rigidbody>())
            {
                RaycastHit rayHit;
                transform.LookAt(hit.transform.position);
                if (Physics.Raycast(transform.position, transform.forward, out rayHit))
                {
                    if (rayHit.transform.gameObject == hit.transform.gameObject)
                    {
                        hit.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
                    }
                }
            }
        }

        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
