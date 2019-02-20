using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour, IMoveable {

    public Vector2 dimensions;
    public Vector3 dimensionOffsets;
    public Material selectedMaterial;

    private Material[] originalMaterials;
    private Renderer[] renderers;

    void Start()
    {
        renderers = gameObject.GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length];
        for(int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;
        }
    }

    private void Update()
    {
        if(GetComponent<Rigidbody>())
        {
            if(transform.position.y <= 0)
            {
                if(GetComponent<IBreakable>() != null)
                {
                    GetComponent<IBreakable>().Break(damageTypes.falling);
                }
            }
        }
    }

    public bool MoveTo(Vector3 newPosition)
    {
        // Switch materials if we haven't already
        if(renderers != null)
        {
            if (renderers[0].material == originalMaterials[0])
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].material = selectedMaterial;
                }
            }
        }

        // Only move if the distance is far enough.  Decreasing this number makes the item move smoother, but can result in false positives.
        if (Vector3.Distance(transform.position, newPosition) < 0.1f)
        {
            return true;
        }

        // Find out which direction we are moving
        Vector3 direction = Vector3.zero;
        if(Mathf.Abs(newPosition.x - transform.position.x) > Mathf.Abs(newPosition.z - transform.position.z))
        {
            if (newPosition.x > transform.position.x)
            {
                direction = new Vector3(1, 0, 0);
            }
            if (newPosition.x < transform.position.x)
            {
                direction = new Vector3(-1, 0, 0);
            }
        }
        else
        {
            if (newPosition.z > transform.position.z)
            {
                direction = new Vector3(0, 0, 1);
            }
            if (newPosition.z < transform.position.z)
            {
                direction = new Vector3(0, 0, -1);
            }
        }

        // Iterate through each dimenion and check in the direction we are moving for obstacles.  Ignore the player.
        if(!CanMoveDirection(direction))
        {
            return false;
        }

        // We didn't hit any obstacles so move to the desired position
        transform.position = newPosition;

        // Iterate through each dimenion and check below the object
        for (int i = 0; i < GetDimensions().x; i++)
        {
            for (int j = 0; j < GetDimensions().y; j++)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + dimensionOffsets + new Vector3(i, 0, j), Vector3.down, out hit))
                {
                    if (hit.distance < 1.2)
                    {
                        return true;
                    }
                }
            }

            // We have no ground beneath us.  Time to fall.
            if (!transform.GetComponent<Rigidbody>())
            {
                gameObject.AddComponent<Rigidbody>();
            }
        }

        return false;
    } // End MoveTo

    public bool CanMoveDirection(Vector3 direction)
    {
        for (int i = 0; i < GetDimensions().x; i++)
        {
            for (int j = 0; j < GetDimensions().y; j++)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + dimensionOffsets + new Vector3(transform.forward.x * i, 0, transform.forward.z * j), direction, out hit))
                {
                    if (hit.distance < 1)
                    {
                        // Ignore the player
                        if (!hit.transform.GetComponent<MovementController>())
                        {
                            // Ignore rigid bodies
                            if (!hit.transform.GetComponent<Rigidbody>())
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        return true;
    }

    public bool Rotate(Vector3 rotation)
    {
        transform.Rotate(0, 45, 0);
        transform.RotateAround(transform.position + new Vector3(GetDimensions().x * transform.forward.x, 1, GetDimensions().y * transform.forward.z) / 4, transform.up, rotation.y);
        transform.Rotate(0, -45, 0);
        SnapIntoPlace();
        return true;
    }

    public Vector2 GetDimensions()
    {
        return dimensions;
    }

    public void SnapIntoPlace()
    {
        float snappedX = transform.position.x > Mathf.RoundToInt(transform.position.x) ?
            Mathf.RoundToInt(transform.position.x) - dimensionOffsets.x : Mathf.RoundToInt(transform.position.x) + dimensionOffsets.x;

        float snappedZ = transform.position.z > Mathf.RoundToInt(transform.position.z) ?
            Mathf.RoundToInt(transform.position.z) - dimensionOffsets.z : Mathf.RoundToInt(transform.position.z) + dimensionOffsets.z;

        transform.position = new Vector3(snappedX, transform.position.y, snappedZ);
    }

    public void ReleaseGrab()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
    }
}
