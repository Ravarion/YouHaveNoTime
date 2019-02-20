using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayTextSwapper : MonoBehaviour {

    public Text howToPlayText;

	void OnEnable()
    {
		if(Input.GetJoystickNames().Length > 0)
        {
            string newText = "<b>How to Play</b>\n\n";
            newText += "Use your super power of amazing speed to save your green clothed comrades.\n\n";
            newText += "Move with the <color=red>Left Joystick</color>\n\n";
            newText += "Pick up people and objects with <color=red>A</color>\n\n";
            newText += "Rotate people and objects with <color=red>LB and RB</color>\n\n";
            newText += "Finish the level early for a higher score by pressing <color=red>Y</color>";
            howToPlayText.text = newText;
        }
        else
        {
            string newText = "<b>How to Play</b>\n\n";
            newText += "Use your super power of amazing speed to save your green clothed comrades.\n\n";
            newText += "Move with <color=red>WASD</color>\n\n";
            newText += "Pick up people and objects with <color=red>F</color>\n\n";
            newText += "Rotate people and objects with <color=red>Z and C</color>\n\n";
            newText += "Finish the level early for a higher score by pressing <color=red>Y</color>";
            howToPlayText.text = newText;
        }
	}
}
