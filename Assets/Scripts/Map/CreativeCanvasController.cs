using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreativeCanvasController : MonoBehaviour {

    private MapMaker mapMaker;

    void Start()
    {
        mapMaker = FindObjectOfType<MapMaker>();
    }

    public void CopyMapToClipboard()
    {
        string mapString = mapMaker.GetMapText(SceneManager.GetActiveScene(), true);

        TextEditor te = new TextEditor{ text = mapString };
        te.SelectAll();
        te.Copy();
    }
}
