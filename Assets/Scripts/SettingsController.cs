using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour {

    public GameObject characterOrientButton;
    public GameObject mapOrientButton;
    public GameObject superHotButton;
    public GameObject quicksilverButton;

	void Start ()
    {
		if(PlayerPrefs.GetInt("MovementStyle") == (int)MovementController.MovementStyle.CHARACTER_ORIENT)
        {
            characterOrientButton.SetActive(false);
        }
        else
        {
            mapOrientButton.SetActive(false);
        }
        if (PlayerPrefs.GetInt("GameMode") == (int)TimeTracker.GameMode.QUICKSILVER)
        {
            quicksilverButton.SetActive(false);
        }
        else
        {
            superHotButton.SetActive(false);
        }
    }
}
