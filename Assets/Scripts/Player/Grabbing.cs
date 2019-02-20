using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbing : MonoBehaviour {

    public bool strongArms = false;

    private GameObject grabbed;
    private Vector3 relativePos;
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public bool UpdateGrabbing()
    {
        if (grabbed)
        {
            if(strongArms)
            {
                if (!grabbed.GetComponent<IMoveable>().MoveTo(transform.position + transform.forward))
                {
                    grabbed.GetComponent<IMoveable>().SnapIntoPlace();
                    return false;
                }
            }
            else if(!grabbed.GetComponent<IMoveable>().MoveTo(transform.position + relativePos))
            {
                grabbed.GetComponent<IMoveable>().SnapIntoPlace();
                return false;
            }
        }
        if(Input.GetButtonDown("Grab/Select"))
        {
            if(grabbed)
            {
                Drop();
            }
            else
            {
                foreach(RaycastHit hit in Physics.RaycastAll(transform.position, transform.forward, 1f))
                {
                    if (hit.distance < 1)
                    {
                        if(gameController.creativeMode && hit.transform.gameObject.GetComponent<MapPieceData>() != null &&
                            hit.transform.gameObject.GetComponent<IMoveable>() == null)
                        {
                            hit.transform.gameObject.AddComponent<Moveable>();
                            hit.transform.gameObject.GetComponent<Moveable>().dimensions = new Vector2(1, 1);
                            hit.transform.gameObject.GetComponent<Moveable>().selectedMaterial = GetComponent<CreativeModeController>().selectedMaterial;
                        }
                        if (hit.transform.gameObject.GetComponent<IMoveable>() != null)
                        {
                            grabbed = hit.transform.gameObject;
                            relativePos = grabbed.transform.position - transform.position;
                            break;
                        }
                    }
                }
            }
        }
        if(Input.GetButtonDown("RotateClock"))
        {
            if(grabbed)
            {
                grabbed.GetComponent<IMoveable>().Rotate(new Vector3(0, 90, 0));
                relativePos = grabbed.transform.position - transform.position;
            }
        }
        if (Input.GetButtonDown("RotateCounterClock"))
        {
            if (grabbed)
            {
                grabbed.GetComponent<IMoveable>().Rotate(new Vector3(0, -90, 0));
                relativePos = grabbed.transform.position - transform.position;
            }
        }
        return true;
	}

    public GameObject GetGrabbed()
    {
        return grabbed;
    }
    
    public void Drop()
    {
        if(grabbed)
        {
            if(grabbed.GetComponent<IMoveable>() != null)
            {
                grabbed.GetComponent<IMoveable>().SnapIntoPlace();
                grabbed.GetComponent<IMoveable>().ReleaseGrab();
                grabbed = null;
            }
        }

    }

    public bool IsGrabbing(GameObject toCheck)
    {
        if(grabbed == toCheck)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Grab(GameObject toGrab)
    {
        if(GetGrabbed())
        {
            return false;
        }

        grabbed = toGrab;

        return true;
    }
}
