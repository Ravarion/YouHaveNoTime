using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MovementController : MonoBehaviour, IBreakable {

    public enum MovementStyle
    {
        MAP_ORIENT,
        CHARACTER_ORIENT
    }

    public float movementSpeed;
    public bool allowConstantInput = false;

    private TimeTracker timeTracker;
    private Vector3 oldPosition;
    private Quaternion desiredRotation;
    private MovementStyle moveStyle = MovementStyle.MAP_ORIENT;
    private Vector3 desiredPos;
    private InputController inputController;
    private GameController gameController;

    void Start()
    {
        inputController = FindObjectOfType<InputController>();
        moveStyle = (MovementStyle)PlayerPrefs.GetInt("MovementStyle");
        desiredPos = transform.position;
        desiredRotation = transform.rotation;
        oldPosition = transform.position;
        timeTracker = FindObjectOfType<TimeTracker>();
        gameController = FindObjectOfType<GameController>();
    }

    public void Break(damageTypes damageType)
    {
        timeTracker.timeLeft = -1;
        GetComponent<Breakable>().Break(damageType);
    }

    public Vector3 SnappedToGrid(Vector3 pos)
    {
        float snappedX = Mathf.RoundToInt(pos.x);
        float snappedZ = Mathf.RoundToInt(pos.z);
        Vector3 snapped = new Vector3(snappedX, pos.y, snappedZ);
        return snapped;
    }

    public void ChangeMovementStyle(int style)
    {
        moveStyle = (MovementStyle)style;
    }

    void Update()
    {
        if(timeTracker.timeLeft <= 0)
        {
            return;
        }

        // Super Hot changes how time flows
        if (timeTracker.superHotMode && !gameController.creativeMode)
        {
            Time.timeScale = Vector3.Distance(transform.position, oldPosition) * 2f;
        }

        oldPosition = transform.position;
        transform.position = Vector3.Lerp(transform.position, desiredPos, movementSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, movementSpeed);
        Vector3 moveDirection = Vector3.zero;
        bool updateDesiredPos = false;
        bool movingIntoGrabbed = false;

        if (inputController.GetOnAxisUp("Vertical")) // KeyCode.W
        {
            if (moveStyle == MovementStyle.CHARACTER_ORIENT)
            {
                moveDirection = transform.forward;
            }
            else
            {
                moveDirection = Vector3.forward;
                transform.LookAt(transform.position + Vector3.forward);
                desiredRotation = transform.rotation;
            }
            CheckMoveDirection(moveDirection, out updateDesiredPos, out movingIntoGrabbed);
        }
        else if (allowConstantInput && inputController.GetAxisUp("Vertical")) // KeyCode.W being held
        {
            if (Vector3.Distance(transform.position, desiredPos) < movementSpeed)
            {
                if (moveStyle == MovementStyle.CHARACTER_ORIENT)
                {
                    moveDirection = transform.forward;
                }
                else
                {
                    moveDirection = Vector3.forward;
                    transform.LookAt(transform.position + Vector3.forward);
                    desiredRotation = transform.rotation;
                }
                CheckMoveDirection(moveDirection, out updateDesiredPos, out movingIntoGrabbed);
            }
        }
        else if (inputController.GetOnAxisDown("Vertical")) // KeyCode.S
        {
            if (moveStyle == MovementStyle.CHARACTER_ORIENT)
            {
                moveDirection = -transform.forward;
            }
            else
            {
                moveDirection = -Vector3.forward;
                transform.LookAt(transform.position - Vector3.forward);
                desiredRotation = transform.rotation;
            }
            CheckMoveDirection(moveDirection, out updateDesiredPos, out movingIntoGrabbed);
        }
        else if (allowConstantInput && inputController.GetAxisDown("Vertical")) // KeyCode.S being held
        {
            if (Vector3.Distance(transform.position, desiredPos) < movementSpeed)
            {
                if (moveStyle == MovementStyle.CHARACTER_ORIENT)
                {
                    moveDirection = -transform.forward;
                }
                else
                {
                    moveDirection = -Vector3.forward;
                    transform.LookAt(transform.position - Vector3.forward);
                    desiredRotation = transform.rotation;
                }
                CheckMoveDirection(moveDirection, out updateDesiredPos, out movingIntoGrabbed);
            }
        }
        else if (inputController.GetOnAxisDown("Horizontal"))// KeyCode.A
        {
            if (moveStyle == MovementStyle.CHARACTER_ORIENT)
            {
                moveDirection = -transform.right;
            }
            else
            {
                moveDirection = -Vector3.right;
                transform.LookAt(transform.position - Vector3.right);
                desiredRotation = transform.rotation;
            }
            CheckMoveDirection(moveDirection, out updateDesiredPos, out movingIntoGrabbed);
        }
        else if (allowConstantInput && inputController.GetAxisDown("Horizontal"))// KeyCode.A being held
        {
            if (Vector3.Distance(transform.position, desiredPos) < movementSpeed)
            {
                if (moveStyle == MovementStyle.CHARACTER_ORIENT)
                {
                    moveDirection = -transform.right;
                }
                else
                {
                    moveDirection = -Vector3.right;
                    transform.LookAt(transform.position - Vector3.right);
                    desiredRotation = transform.rotation;
                }
                CheckMoveDirection(moveDirection, out updateDesiredPos, out movingIntoGrabbed);
            }
        }
        else if (inputController.GetOnAxisUp("Horizontal")) // KeyCode.D
        {
            if (moveStyle == MovementStyle.CHARACTER_ORIENT)
            {
                moveDirection = transform.right;
            }
            else
            {
                moveDirection = Vector3.right;
                transform.LookAt(transform.position + Vector3.right);
                desiredRotation = transform.rotation;
            }
            CheckMoveDirection(moveDirection, out updateDesiredPos, out movingIntoGrabbed);
        }
        else if (allowConstantInput && inputController.GetAxisUp("Horizontal")) // KeyCode.D being held
        {
            if (Vector3.Distance(transform.position, desiredPos) < movementSpeed)
            {
                if (moveStyle == MovementStyle.CHARACTER_ORIENT)
                {
                    moveDirection = transform.right;
                }
                else
                {
                    moveDirection = Vector3.right;
                    transform.LookAt(transform.position + Vector3.right);
                    desiredRotation = transform.rotation;
                }
                CheckMoveDirection(moveDirection, out updateDesiredPos, out movingIntoGrabbed);
            }
        }

        // Check if we're about to jump off the map.  Allowed in creative mode.
        if (!gameController.creativeMode)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + moveDirection, Vector3.down, out hit))
            {
                if (hit.distance > 1)
                {
                    // Nothing to stand on, don't move
                    return;
                }
            }
            else
            {
                // Nothing to stand on, don't move
                return;
            }
        }

        GetComponent<Grabbing>().UpdateGrabbing();
        if(updateDesiredPos)
        {
            updateDesiredPos = false;
            // Make sure we're grabbing something before we try to access
            if(GetComponent<Grabbing>().GetGrabbed())
            {
                //Check if we're running into the object we're grabbing
                if(movingIntoGrabbed)
                {
                    // Check that the item we're holding can move in the direction we're trying to move, otherwise don't move
                    if (GetComponent<Grabbing>().GetGrabbed().GetComponent<IMoveable>().CanMoveDirection(moveDirection))
                    {
                        updateDesiredPos = true;
                    }
                }
                else
                {
                    // Check that the item we're holding can move in the direction we're trying to move, otherwise drop it
                    if (!GetComponent<Grabbing>().GetGrabbed().GetComponent<IMoveable>().CanMoveDirection(moveDirection))
                    {
                        GetComponent<Grabbing>().Drop();
                    }
                    updateDesiredPos = true;
                }
            }
            else
            {
                updateDesiredPos = true;
            }
        }
        if(updateDesiredPos)
        {
            desiredPos = transform.position + moveDirection;
            desiredPos = SnappedToGrid(desiredPos);
        }

        if (inputController.GetOnAxisDown("LookHorizontal"))// KeyCode.Q
        {
            Vector3 newRotation = desiredRotation.eulerAngles + new Vector3(0, -90, 0);
            desiredRotation = Quaternion.Euler(newRotation);
        }
        if (inputController.GetOnAxisUp("LookHorizontal"))// KeyCode.E
        {
            Vector3 newRotation = desiredRotation.eulerAngles + new Vector3(0, 90, 0);
            desiredRotation = Quaternion.Euler(newRotation);
        }
    } // End Update

    // Updates the two out params to be used
    private void CheckMoveDirection(Vector3 moveDirection, out bool updateDesiredPos, out bool movingIntoGrabbed)
    {
        updateDesiredPos = true;
        movingIntoGrabbed = false;
        // Check if we're about to walk into something that we're not grabbing
        if (!gameController.creativeMode)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position, moveDirection, 1f);

            foreach(RaycastHit hit in hits)
            {
                if (hit.distance < 1)
                {
                    if (!GetComponent<Grabbing>().IsGrabbing(hit.transform.gameObject))
                    {
                        //Ignore anything with a rigidbody, ignore anything without Map Data
                        if (!hit.transform.GetComponent<Rigidbody>() && hit.transform.GetComponent<MapPieceData>())
                        {
                            // We ran into something that we're not grabbing
                            updateDesiredPos = false;
                        }
                    }
                    else
                    {
                        // We're walking into the item we're grabbing
                        movingIntoGrabbed = true;
                    }
                }
            }
        }
    }
}
