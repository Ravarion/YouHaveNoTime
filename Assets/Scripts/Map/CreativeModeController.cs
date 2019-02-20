using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreativeModeController : MonoBehaviour {

    public Material selectedMaterial;

    private GameController gameController;
    private InputController inputController;
    private CreativeUIController creativeUIController;
    private GameObject toSpawn;
    private GameObject[][] spawnLists;
    private MapMaker mapMaker;
    private Grabbing grabbing;
    private int listIterator = 0;
    private int objIterator = 0;
    private GameObject map;

	void Start ()
    {
        gameController = FindObjectOfType<GameController>();
        inputController = FindObjectOfType<InputController>();
        mapMaker = FindObjectOfType<MapMaker>();
        grabbing = GetComponent<Grabbing>();
        map = GameObject.Find("Map");
        if(map == null)
        {
            map = new GameObject("Map");
            map.transform.position = Vector3.zero;
        }
        spawnLists = new GameObject[4][];
        spawnLists[0] = mapMaker.wallList;
        spawnLists[1] = mapMaker.actorList;
        spawnLists[2] = mapMaker.hazardList;
        spawnLists[3] = mapMaker.floorList;

        creativeUIController = FindObjectOfType<CreativeUIController>();
        if(gameController.creativeMode)
        {
            SendUIUpdate();
        }
    }
	
	void Update ()
    {
        if(!gameController.creativeMode)
        {
            return;
        }

        // Spawn objects and floors with the Y button
        if(Input.GetButtonDown("Time Restore/Spawn"))
        {
            toSpawn = Instantiate(spawnLists[listIterator][objIterator], new Vector3(transform.position.x, 1, transform.position.z), transform.rotation);
            
            // Spawn lower if the object is a floor
            if(toSpawn.GetComponent<MapPieceData>().tileType == TileType.FLOOR)
            {
                toSpawn.transform.position -= Vector3.up;
            }

            toSpawn.transform.parent = map.transform;
            if (!toSpawn.GetComponent<Moveable>())
            {
                Moveable toSpawnMoveable = toSpawn.AddComponent<Moveable>();
                toSpawnMoveable.dimensions = new Vector2(1, 1);
                toSpawnMoveable.selectedMaterial = selectedMaterial;
            }
            // If this space is already occupied, delete what we're trying to spawn
            if (!toSpawn.GetComponent<Moveable>().CanMoveDirection(transform.forward))
            {
                Destroy(toSpawn);
            }
            else
            {
                toSpawn.transform.position += transform.forward;
                toSpawn.GetComponent<Moveable>().SnapIntoPlace();
            }
            toSpawn = null;
        }

        // Destroy objects and floors with the X button
        if (Input.GetButtonDown("Cancel"))
        {
            GameObject grabbed = grabbing.GetGrabbed();
            if (grabbed)
            {
                grabbing.Drop();
                Destroy(grabbed);
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit) && hit.distance < 1.2)
                {
                    Destroy(hit.transform.gameObject);
                }
                else if (Physics.Raycast(transform.position + transform.forward, Vector3.down, out hit) && hit.distance < 1.2)
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }

        // Iterate forward through list of spawnables
        if(inputController.GetOnAxisDown("DpadHorizontal"))
        {
            objIterator += 1;
            if (objIterator >= spawnLists[listIterator].Length)
            {
                objIterator = 0;
            }
            SendUIUpdate();
        }

        // Iterate backward through list of spawnables
        if (inputController.GetOnAxisUp("DpadHorizontal"))
        {
            objIterator -= 1;
            if (objIterator < 0)
            {
                objIterator = spawnLists[listIterator].Length - 1;
            }
            SendUIUpdate();
        }

        // Switch to the next list of spawnables. Start at the beginning of the list.
        if (inputController.GetOnAxisDown("DpadVertical"))
        {
            listIterator -= 1;
            objIterator = 0;
            if (listIterator < 0)
            {
                listIterator = spawnLists.Length - 1;
            }
            SendUIUpdate();
        }

        // Switch to the previous list of spawnables.  Start at the beginning of the list.
        if (inputController.GetOnAxisUp("DpadVertical"))
        {
            listIterator += 1;
            objIterator = 0;
            if (listIterator >= spawnLists.Length)
            {
                listIterator = 0;
            }
            SendUIUpdate();
        }
    } // End Update

    private void SendUIUpdate()
    {
        List<GameObject> toSend = new List<GameObject>();
        toSend.AddRange(spawnLists[listIterator].Skip(objIterator));
        toSend.AddRange(spawnLists[listIterator].Take(objIterator));

        creativeUIController.DisplayObjects(toSend.ToArray());
    }
}
