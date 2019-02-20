using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreativeUIController : MonoBehaviour {

    public GameObject spawnUIStudio;
    public GameObject creativeCanvasPrefab;
    public RenderTexture[] renderTextures;

    public bool levelSelectionMode = false;

    private GameObject creativeCanvas;
    private GameObject[] spawnStudios;

    void Start()
    {
        spawnStudios = new GameObject[renderTextures.Length];
    }

    public void DisplayObjects(GameObject[] toDisplay)
    {
        for (int i = 0; i < toDisplay.Length; i++)
        {
            DisplayObject(toDisplay[i], i);
        }

        // Delete any studios that aren't needed.
        for(int i = toDisplay.Length; i < spawnStudios.Length; i++)
        {
            if(spawnStudios[i] != null)
            {
                Destroy(spawnStudios[i]);
            }
        }
    }

    private void DisplayObject(GameObject toDisplay, int index)
    {
        // If we don't have any spawnStudios left to display, back out
        if(index >= spawnStudios.Length)
        {
            return;
        }

        // If we haven't spawned the UI yet
        if(!levelSelectionMode && creativeCanvas == null)
        {
            creativeCanvas = Instantiate(creativeCanvasPrefab);
        }

        // Spawn a new studio if one doesn't exist already
        if(spawnStudios[index] == null)
        {
            spawnStudios[index] = Instantiate(spawnUIStudio, new Vector3(500 + (50 * index), 0, 500), Quaternion.identity);
            spawnStudios[index].GetComponentInChildren<Camera>().targetTexture = renderTextures[index];
        }

        // Find the DisplayStand
        GameObject displayStand = spawnStudios[index].transform.GetChild(3).gameObject;

        // If the DisplayStand already has something to show, check if it's the same as what we're trying to give it
        if(displayStand.transform.childCount > 0)
        {
            // Check if it's the same name, ignoring the potential (Clone) at the end
            if(displayStand.transform.GetChild(0).name.Split('(')[0] == toDisplay.name)
            {
                // If it's the same object, we don't need to do anything
                return;
            }
            else
            {
                // Delete the object on display
                Destroy(displayStand.transform.GetChild(0).gameObject);
            }
        }
        
        // Spawn the new object to display
        GameObject spawned = Instantiate(toDisplay, spawnStudios[index].transform.position, Quaternion.identity);
        spawned.transform.SetParent(displayStand.transform);

        // Destroy the rigidbody so that the object does not fall
        if(spawned.GetComponentInChildren<Rigidbody>())
        {
            Destroy(spawned.GetComponentInChildren<Rigidbody>());
        }
        // Don't let the player spawner actually spawn the player
        if(spawned.GetComponentInChildren<SpawnPlayer>())
        {
            Destroy(spawned.GetComponentInChildren<SpawnPlayer>());
        }

        // Remove Map Piece Data so that it doesn't get saved to the level file
        if (spawned.GetComponentInChildren<MapPieceData>())
        {
            Destroy(spawned.GetComponentInChildren<MapPieceData>());
        }

        // Rotate the display stand.  This is easier than trying to rotate the object that we spawned
        displayStand.transform.Rotate(new Vector3(0, 135, 0));
    } // End DisplayObject
}
