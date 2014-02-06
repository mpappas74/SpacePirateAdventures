using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour
//This script creates the main menu.
{
	
	//Main determines whether we are at the basic main menu (Play vs Settings vs High Scores).
	//Similarly, settingsMenu, levelMenu, and soundTest tell us we are in other deeper menus.
	//backButton determines whether there currently needs to be a button allowing return to the main menu.
	public bool main;
	public bool backButton;
	public bool pressed1;
	public bool pressed2;
	public bool settingsMenu;
	public bool levelMenu;
	public bool soundTest;
	//The audioSources are for the sound test. We need to know what they are, what they are called, etc.
	public AudioSource level1;
	public AudioSource level2;
	public AudioSource level3;
	public AudioSource boss1;
	public AudioSource boss2;
	public AudioSource shop;
	private AudioSource[] allSources;
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

	//The guiText object will display information such as "There are no settings yet."
	private string text = "";
	public GUIStyle theGuiTextStyle;
	//The titleStyle determines the format of the title.
	public GUIStyle titleStyle;
	
	//This is for the level grid, which will automatically nicely format our level selection screen.
	private int levelGridInt = -1;
	private string[] levelStrings = {"1", "2", "3", "4"};

	void Start ()
	{
		allSources = new AudioSource[6]{level1, level2, level3, boss1, boss2, shop};
	}

	//Stop all music playing in SoundTest as well as pausing the main menu music.
	private void stopMusic ()
	{
		foreach (AudioSource a in allSources) {
			a.Stop ();
		}
		audio.Pause ();
	}

	//Essentially unncessary. Might move more logic here eventually, since Update() is called less often than OnGUI() so logic
	//should optimally be executed here as much as possible.
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
		//We also declare a currently empty text box which we will use if we need to put text onscreen anywhere in the menu.
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
		//Notice that it is only in these few options that we need to declare the backButton bool as true, since otherwise it just holds its value.
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
			//The selectionGrid will allow you to choose exactly one of a set of buttons. levelGridInt is initialized to -1 so no level starts out selected.
			int tempInt = GUI.SelectionGrid (new Rect (.5f * Screen.width - 125, .4f * Screen.height, 250, .4f * Screen.height), levelGridInt, levelStrings, 4);
			//This if statement is used to prevent us from running the remaining logic too often, especially in OnGUI.
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
						Application.LoadLevel ("Level3");
						break;
					case 3:
						text = "There is no level 4 yet!";
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
				//Fun fact - in the text above, if you count the letters, punctuation, and spaces as characters, there are exactly 74.
			}
		} else if (soundTest) {

			//Another selection grid. This time soundTestGridInt is initialized to 0 so MainMenu music starts out selected.
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
