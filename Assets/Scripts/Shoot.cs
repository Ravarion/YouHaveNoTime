using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    public GameObject toShoot;
    public AudioClip shotSoundEffect;
    public Transform shotOrigin;
    public int numberOfShots;
    public float shotArc;
    public float shotVelocity;
    public float shotFrequency;
    public float shotDelay = 0;
    public bool shootOnStart = true;

    public void StartShooting()
    {
        StartCoroutine("Shooting");
    }

    private void Start()
    {
        if (shootOnStart)
        {
            StartCoroutine("Shooting");
        }
    }

    IEnumerator Shooting()
    {
        yield return new WaitForSeconds(shotDelay);

        while(numberOfShots != 0)
        {
            yield return new WaitForSeconds(shotFrequency);
            Fire();
            numberOfShots -= 1;
        }
    }

    void Fire()
    {
        FindObjectOfType<AudioManager>().PlayClip(shotSoundEffect, transform.position);
        GameObject bullet = Instantiate(toShoot, shotOrigin.position, Quaternion.identity);
        Vector3 trajectory = new Vector3(shotOrigin.forward.x * shotVelocity, shotArc * shotVelocity, shotOrigin.forward.z * shotVelocity);
        bullet.GetComponent<Rigidbody>().AddForce(trajectory, ForceMode.Impulse);
    }
}
