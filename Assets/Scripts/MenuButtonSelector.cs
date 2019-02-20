using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButtonSelector : MonoBehaviour {

    public Button defaultSelection;
    public Button secondSelection;

    public bool onEnable = false;
    public bool onStart = true;

    private InputController inputController;

    void Start()
    {
        inputController = FindObjectOfType<InputController>();
        if(!onStart)
        {
            return;
        }
        Button toSelect = defaultSelection;
        if(!defaultSelection.IsActive() && secondSelection != null)
        {
            toSelect = secondSelection;
        }
        if (toSelect != EventSystem.current.currentSelectedGameObject)
        {
            // Due to a bug in Unity, it is necessary to disable and enable the button before selecting
            toSelect.gameObject.SetActive(false);
            toSelect.gameObject.SetActive(true);

            toSelect.Select();
        }
    }

    private void OnEnable()
    {
        if(!onEnable)
        {
            return;
        }
        Button toSelect = defaultSelection;
        if (!defaultSelection.IsActive() && secondSelection != null)
        {
            toSelect = secondSelection;
        }
        if (toSelect != EventSystem.current.currentSelectedGameObject)
        {
            // Due to a bug in Unity, it is necessary to disable and enable the button before selecting
            toSelect.gameObject.SetActive(false);
            toSelect.gameObject.SetActive(true);

            toSelect.Select();
        }
    }

    void Update()
    {
        if(inputController.GetOnAxisUp("Vertical") || inputController.GetOnAxisDown("Vertical") ||
            inputController.GetOnAxisUp("Horizontal") || inputController.GetOnAxisDown("Horizontal"))
        {
            if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy)
            {
                print("Error: No button selected. Selecting default.");
                defaultSelection.Select();
            }
        }
    }
}
