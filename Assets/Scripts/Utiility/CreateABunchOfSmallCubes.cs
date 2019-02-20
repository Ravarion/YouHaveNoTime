using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateABunchOfSmallCubes : MonoBehaviour{

    public GameObject smallCube;
    public Vector3 dimensions;

    private void Start()
    {
        for(int i = 0; i < dimensions.x; i++)
        {
            for(int j = 0; j < dimensions.y; j++)
            {
                for(int k = 0; k < dimensions.z; k++)
                {
                    Instantiate(smallCube, new Vector3(i*.1f, j*.1f, k*.1f), Quaternion.identity);
                }
            }
        }
        Time.timeScale = 0;
    }
}
