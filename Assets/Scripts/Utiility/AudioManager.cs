using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public GameObject audioPrefab;

    public void PlayClip(AudioClip clip, Vector3 pos)
    {
        GameObject sourceObj = Instantiate(audioPrefab, pos, Quaternion.identity);
        sourceObj.GetComponent<AudioSource>().pitch = Time.timeScale < 1 ? 0.5f : 1f;
        sourceObj.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
