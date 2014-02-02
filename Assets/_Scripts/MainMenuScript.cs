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
	public bool backButton;
	public bool pressed1;
	public bool pressed2;
	public bool settingsMenu;
	public bool levelMenu;
	public bool soundTest;
	public AudioSource level1;
	public AudioSource level2;
	public AudioSource level3;
	public AudioSource boss1;
	public AudioSource boss2;
	public AudioSource shop;

	//The guiText object will display information such as "There are no settings yet."
	private string text = "";
	public GUIStyle theGuiTextStyle;
	//The titleStyle determines the format of the title.
	public GUIStyle titleStyle;
	private AudioSource[] allSources;
	private AudioSource audio;
	private int soundTestGridInt = 0;
	private string[] soundTestStrings = {
		"MainMenu",
		"Level1",
		"Level2",
		"Level3",
		"Boss1",
		"Boss2",
		"Shop"
	};
	private int levelGridInt = -1;
	private string[] levelStrings = {"1", "2", "3"};

	void Start ()
	{
		allSources = new AudioSource[6]{level1, level2, level3, boss1, boss2, shop};
		audio = GetComponent<AudioSource> ();
	}

	private void stopMusic ()
	{
		foreach (AudioSource a in allSources) {
			a.Stop ();
		}
		audio.Pause ();
	}

	void Update ()
	{

		if (pressed2) {
			text = "There are no high scores yet. We need a game first.";
			pressed2 = false;
		}
	}
	
	void OnGUI ()
	{
		//Generate the buttons in locations based on screen size to keep a consistent positioning.
		//Start by declaring the title just as a text box.

		GUI.Box (new Rect (.5f * Screen.width - 200, .1f * Screen.height, 400, .15f * Screen.height), "Space Pirate Adventures!", titleStyle);
		GUI.Box (new Rect (.5f * Screen.width - 200, .25f * Screen.height, 400, .7f * Screen.height), text, theGuiTextStyle);
		
		//If we are not on the main menu, we need a back button to return to it.
		if (backButton) {
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .8f * Screen.height, 200, .12f * Screen.height), "Return to Main Menu")) {
				main = true;
				backButton = false;
				levelMenu = false;
				settingsMenu = false;
				soundTest = false;
				text = "";
				stopMusic ();
				audio.Play ();
			}
		}
		//If we are on the main menu, show it.
		if (main) {
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .35f * Screen.height, 200, .12f * Screen.height), "Play Game")) {
				main = false;
				levelMenu = true;
				backButton = true;
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .5f * Screen.height, 200, .12f * Screen.height), "Settings")) {
				settingsMenu = true;
				main = false;
				backButton = true;
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .65f * Screen.height, 200, .12f * Screen.height), "High Score")) {
				main = false;
				text = "There are no high scores yet. We need a game first.";
				backButton = true;
			} 
		} else if (levelMenu) {
			int tempInt = GUI.SelectionGrid (new Rect (.5f * Screen.width - 125, .4f * Screen.height, 250, .4f * Screen.height), levelGridInt, levelStrings, 3);
			if (tempInt != levelGridInt) {
				if (GameControllerScript.Instance.getCurrentUnlockedLevel () <= tempInt) {
					text = "You haven't unlocked level " + (tempInt + 1).ToString () + " yet.";
				} else {
					switch (tempInt) {
					case 0:
						Application.LoadLevel ("Level1");
						break;
					case 1:
						Application.LoadLevel ("Level2");
						break;
					case 2:
						text = "There is no level 3 yet!";
						break;
					}
				}
			}
			levelGridInt = tempInt;
		} else if (settingsMenu) {
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .35f * Screen.height, 200, .12f * Screen.height), "Sound Test")) {
				settingsMenu = false;
				soundTest = true;
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .5f * Screen.height, 200, .12f * Screen.height), "Credits")) {
				settingsMenu = false;
				text = "This game was made by Mark, Mike, Steven, and Terry. Find the best number.";
			}
		} else if (soundTest) {
			int tempInt = GUI.SelectionGrid (new Rect (.5f * Screen.width - 125, .3f * Screen.height, 250, .5f * Screen.height), soundTestGridInt, soundTestStrings, 3);
			if (tempInt != soundTestGridInt) {
				stopMusic ();
				switch (tempInt) {
				case 0:
					audio.Play ();
					break;
				case 1:
					level1.Play ();
					break;
				case 2:
					level2.Play ();
					break;
				case 3:
					level3.Play ();
					break;
				case 4:
					boss1.Play ();
					break;
				case 5:
					boss2.Play ();
					break;
				case 6:
					shop.Play ();
					break;
				}
			}
			soundTestGridInt = tempInt;

		}
		
	}
}
