using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour
//This script creates the main menu.
{
	
	//Main determines whether we are at the basic main menu (Play vs Settings vs High Scores).
	//view determines whether all options are currently visible or if we are trying to display text.
	//(Might help to play around in the main menu to see how this works).
	//backButton determines whether there currently needs to be a button allowing return to the main menu.
	public bool main;
	public bool view;
	public bool backButton;
	public bool pressed1;
	public bool pressed2;

	//The guiText object will display information such as "There are no settings yet."
	private GUIText theGuiText;
	//The titleStyle determines the format of the title.
	public GUIStyle titleStyle;

	void Start ()
	{
		//Get access to our GUIText.
		theGuiText = gameObject.GetComponent<GUIText> ();
	}

	void Update ()
	{
		//If we just pressed something requiring text, update the text.
		if (pressed1) {
			theGuiText.text = "There are no settings yet, jerk.";
			backButton = true;
			pressed1 = false;
		}
		if (pressed2) {
			theGuiText.text = "There are no high scores yet. We need a game first.";
			backButton = true;
			pressed2 = false;
		}
	}
	
	void OnGUI ()
	{
		//Generate the buttons in locations based on screen size to keep a consistent positioning.
		//Start by declaring the title just as a text box.

		GUI.Box (new Rect (.5f * Screen.width - 200, .1f * Screen.height, 400, .18f * Screen.height), "Space Pirate Adventures!", titleStyle);
		
		//If we are not on the main menu, we need a back button to return to it.
		if (backButton) {
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .8f * Screen.height, 200, .12f * Screen.height), "Return to Main Menu")) {
				view = true;
				main = true;
				backButton = false;
				theGuiText.text = "";
				
			}
		}
		//If we are looking at more button options right now, display them.
		if (view) {
			//If we are on the main menu, show it.
			if (main) {
				if (GUI.Button (new Rect (.5f * Screen.width - 100, .35f * Screen.height, 200, .12f * Screen.height), "Play Game")) {
					main = false;
					backButton = true;
				}
				if (GUI.Button (new Rect (.5f * Screen.width - 100, .5f * Screen.height, 200, .12f * Screen.height), "Settings")) {
					pressed1 = true;
					view = false;
				}
				if (GUI.Button (new Rect (.5f * Screen.width - 100, .65f * Screen.height, 200, .12f * Screen.height), "High Score")) {
					pressed2 = true;
					view = false;
				} 
			} else {
				//If we are not on the main menu, we must be on the Level Selection screen.
				if (GUI.Button (new Rect (.5f * Screen.width - 100, .35f * Screen.height, 200, .12f * Screen.height), "Level 1")) {
					Application.LoadLevel ("Level1");
				} 
				if (GUI.Button (new Rect (.5f * Screen.width - 100, .5f * Screen.height, 200, .12f * Screen.height), "Level 2")) {
					if (GameControllerScript.getCurrentUnlockedLevel() < 2) {
						theGuiText.text = "You haven't unlocked level 2 yet!";
					} else {
						Application.LoadLevel("Level2");
					}
					view = false;
				}
				if (GUI.Button (new Rect (.5f * Screen.width - 100, .65f * Screen.height, 200, .12f * Screen.height), "Level 3")) {
					theGuiText.text = "There is no level 3 yet!";
					view = false;
				} 
			}
		}
	}
}
