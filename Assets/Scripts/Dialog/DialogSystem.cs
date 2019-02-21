using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour {

	public GameObject dialogContainer;
	public GameObject nextButton;
	public Image talkerLeft;
	public Image talkerRight;
	public Color leftColor;
	public Color rightColor;
	public Text nameLeft;
	public Text nameRight;
	public Text message;
	public Text choice1;
	public Text choice2;
	public Text choice3;

	private GameObject subjectLeft;
	private GameObject subjectRight;
	private TextAsset dialogFile;
	private int iterator;
	private string[] dialogLine;

	private Text currentText;

	private string[] choiceActions = new string[3];


	public void SetDialogFile(TextAsset p_dialog){
		dialogFile = p_dialog;
		if(dialogFile != null)
		{
			dialogLine = (dialogFile.text.Split('\n'));
		}
	}

	public void NextMessage(){
		message.text = "";
		choice1.text = "";
		choice2.text = "";
		choice3.text = "";
		if (dialogLine [iterator].IndexOf ("@") == 0) {
			iterator++;
			NextMessage ();
			return;
		} else {
			string[] dialogWord = dialogLine [iterator].Split (' ');
			for (int i = 0; i < dialogWord.Length; i++) {
				switch (dialogWord[i]) {
				case "#DimLeft":
					talkerLeft.color = new Color(leftColor.r/2, leftColor.g/2, leftColor.b/2, leftColor.a);
					break;
				case "#DimRight":
					talkerRight.color = new Color(rightColor.r/2, rightColor.g/2, rightColor.b/2, rightColor.a);
					break;
				case "#BrightLeft":
					talkerLeft.color = new Color(leftColor.r, leftColor.g, leftColor.b, leftColor.a);
					break;
				case "#BrightRight":
					talkerRight.color = new Color(rightColor.r, rightColor.g, rightColor.b, rightColor.a);
					break;
				case "#SetChoices":
					message.gameObject.SetActive (false);
					nextButton.SetActive (false);
					break;
				case "#SetDialog":
					message.gameObject.SetActive (true);
					choice1.gameObject.SetActive (false);
					choice2.gameObject.SetActive (false);
					choice3.gameObject.SetActive (false);
					currentText = message;
					nextButton.SetActive (true);
					break;
				case "#Choice1Text":
					currentText = choice1;
					break;
				case "#Choice2Text":
					currentText = choice2;
					break;
				case "#Choice3Text":
					currentText = choice3;
					break;
				case "#Choice1":
					choice1.gameObject.SetActive (true);
					choiceActions [0] = dialogWord [i + 1];
					i++;
					break;
				case "#Choice2":
					choice2.gameObject.SetActive (true);
					choiceActions [1] = dialogWord [i + 1];
					i++;
					break;
				case "#Choice3":
					choice3.gameObject.SetActive (true);
					choiceActions [2] = dialogWord [i + 1];
					i++;
					break;
				case "#CloseDialog":
					EndDialog ();
					return;
				case "#GetName":
					//currentText.text += " " + subjectRight.GetComponent<Human>().characterName;
					break;
				case "#GetJob":
					//if (subjectRight.GetComponent<Human> ().job != null) {
					//	currentText.text += " " + subjectRight.GetComponent<Human> ().job.name;
					//} else {
					//	currentText.text += " " + "unemployed... and I'm horribly bored.";
					//}
					break;
				case ".":
					currentText.text += ".";
					break;
				default:
					if (dialogWord [i].Contains ("#goto:")) {
						string locationName = dialogWord [i].Split (':') [1];
						for (int j = 0; j < dialogLine.Length; j++) {
							if (dialogLine [i] == "@" + locationName) {
								iterator = i;
								break;
							}
						}
					} else {
						if (i == 0) {
							currentText.text += dialogWord [i];
						} else {
							currentText.text += " " + dialogWord [i];
						}
					}
					break;
				}
			}
			iterator++;
		}
		if (message.text == "" && choice1.text == "" && choice2.text == "" && choice3.text == "") {
			NextMessage ();
			return;
		}
	}

	public void SetTalkerLeft(GameObject subject){
		subjectLeft = subject;
		talkerLeft.sprite = subjectLeft.GetComponent<SpriteRenderer> ().sprite;
		talkerLeft.color = subjectLeft.GetComponent<SpriteRenderer>().color;
		leftColor = talkerLeft.color;
		//nameLeft.text = "<color=blue>"+subjectLeft.GetComponent<Human> ().characterName+"</color>";
	}

	public void SetTalkerRight(GameObject subject){
		subjectRight = subject;
		talkerRight.sprite = subjectRight.GetComponent<SpriteRenderer> ().sprite;
		talkerRight.color = subjectRight.GetComponent<SpriteRenderer>().color;
		rightColor = talkerRight.color;
		//nameRight.text = "<color=blue>"+subjectRight.GetComponent<Human> ().characterName+"</color>";
	}

	public void SetMessage(string newMessage){
		message.text = newMessage;
	}

	public void EndDialog(){
		dialogContainer.SetActive (false);
	}

	public void StartDialog(){
		iterator = 0;
		message.text = "";
		choice1.text = "";
		choice2.text = "";
		choice3.text = "";
		currentText = message;
		dialogContainer.SetActive (true);
		message.gameObject.SetActive (true);
		choice1.gameObject.SetActive (false);
		choice2.gameObject.SetActive (false);
		choice3.gameObject.SetActive (false);
		nextButton.SetActive (true);
		NextMessage();
	}

	public void ChooseOption(int optionNumber){
		if(choiceActions[optionNumber].Contains("#goto:")){
			string locationName = choiceActions [optionNumber].Split (':') [1];
			for(int i = 0; i < dialogLine.Length; i++){
				if(dialogLine[i] == "@" + locationName) {
					iterator = i;
					NextMessage ();
					break;
				}
			}
		}
	}
}