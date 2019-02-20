using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour, ITorchable {

    public GameObject fuse;
    private bool torched = false;

    public void Torch()
    {
        if(!torched)
        {
            GetComponent<Shoot>().StartShooting();
            torched = true;
            fuse.SetActive(true);
        }
    }
}
