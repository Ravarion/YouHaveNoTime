using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPieceData : MonoBehaviour {

    public MapPiece mapPiece;
    public TileType tileType;
    public string objectName;

    public void UpdateData()
    {
        mapPiece.posX = Mathf.RoundToInt(transform.position.x);
        mapPiece.posY = Mathf.RoundToInt(transform.position.y);
        mapPiece.posZ = Mathf.RoundToInt(transform.position.z);
        mapPiece.rotX = Mathf.RoundToInt(transform.eulerAngles.x);
        mapPiece.rotY = Mathf.RoundToInt(transform.eulerAngles.y);
        mapPiece.rotZ = Mathf.RoundToInt(transform.eulerAngles.z);
        mapPiece.tileType = tileType;
        mapPiece.objName = objectName;
    }
}
