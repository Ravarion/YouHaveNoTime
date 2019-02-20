using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour, IBreakable {

    public GameObject replaceOnBreak;
    public AudioClip soundEffectOnBreak;
    public damageTypes[] brokenBy;
    public float moveBrokenYUp = 0;

    public void Break(damageTypes damageType)
    {
        for(int i = 0; i < brokenBy.Length; i++)
        {
            if(damageType == brokenBy[i])
            {
                if (soundEffectOnBreak)
                {
                    FindObjectOfType<AudioManager>().PlayClip(soundEffectOnBreak, transform.position);
                }
                GetComponent<Collider>().enabled = false;
                Instantiate(replaceOnBreak, transform.position + Vector3.up * moveBrokenYUp, transform.rotation);
                Destroy(gameObject);
                break;
            }
        }
    }
}
