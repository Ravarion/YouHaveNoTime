using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollide : MonoBehaviour
{
    public damageTypes damageType;
    public float radius;
    public float radiusOffset;
    public float velocityRequired;

    private float timeActive = 0;
    private bool isActive = true;

    void FixedUpdate()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude > velocityRequired)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, GetComponent<Rigidbody>().velocity, out hit))
            {
                if (hit.distance < radius + radiusOffset)
                {
                    if (hit.transform.gameObject.GetComponent<IBreakable>() != null)
                    {
                        hit.transform.gameObject.GetComponent<IBreakable>().Break(damageType);
                        isActive = false;
                    }
                }
            }
        }
        timeActive += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isActive)
        {
            if (GetComponent<Rigidbody>().velocity.magnitude > velocityRequired || timeActive < 0.01f)
            {
                if (collision.transform.gameObject.GetComponent<IBreakable>() != null)
                {
                    collision.transform.gameObject.GetComponent<IBreakable>().Break(damageType);
                    isActive = false;
                }
            }
        }
    }
}
