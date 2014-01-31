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

	void Start ()
	{
		allSources = new AudioSource[6]{level1, level2, level3, boss1, boss2, shop};
		audio = GetComponent<AudioSource>();
	}

	private void stopMusic(){
		foreach(AudioSource a in allSources){
			a.Stop();
		}
		audio.Stop();
	}

	void Update ()
	{

		if (pressed2) {
			text = "There are no high scores yet. We need a game first.";
			backButton = true;
			pressed2 = false;
		}
	}
	
	void OnGUI ()
	{
		//Generate the buttons in locations based on screen size to keep a consistent positioning.
		//Start by declaring the title just as a text box.

		GUI.Box (new Rect (.5f * Screen.width - 200, .1f * Screen.height, 400, .18f * Screen.height), "Space Pirate Adventures!", titleStyle);
		GUI.Box (new Rect (.5f * Screen.width - 200, .6f * Screen.height, 400, .18f * Screen.height), text , theGuiTextStyle);
		
		//If we are not on the main menu, we need a back button to return to it.
		if (backButton) {
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .8f * Screen.height, 200, .12f * Screen.height), "Return to Main Menu")) {
				main = true;
				backButton = false;
				levelMenu = false;
				settingsMenu = false;
				soundTest = false;
				text = "";
				stopMusic();
				audio.Play();
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
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .65f * Screen.height, 200, .12f * Screen.height), "High Score")) {
				main = false;
				text = "There are no high scores yet. We need a game first.";
				backButton = true;
			} 
		} else if(levelMenu) {
			//If we are not on the main menu, we must be on the Level Selection screen.
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .35f * Screen.height, 200, .12f * Screen.height), "Level 1")) {
				Application.LoadLevel ("Level1");
			} 
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .5f * Screen.height, 200, .12f * Screen.height), "Level 2")) {
				if (GameControllerScript.Instance.getCurrentUnlockedLevel () < 2) {
					text = "You haven't unlocked level 2 yet!";
					levelMenu = false;
				} else {
					Application.LoadLevel ("Level2");
				}
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .65f * Screen.height, 200, .12f * Screen.height), "Level 3")) {
				text = "There is no level 3 yet!";
				levelMenu = false;
			} 
		} else if(settingsMenu){
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .35f * Screen.height, 200, .12f * Screen.height), "Sound Test")) {
				settingsMenu = false;
				soundTest = true;
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .5f * Screen.height, 200, .12f * Screen.height), "Credits")) {
				settingsMenu = false;
				text = "This game was made by Mark, Mike, Steven, and Terry. Find the best number.";
				backButton = true;
			}
		} else if(soundTest){
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .13f * Screen.height, 200, .12f * Screen.height), "Level1")) {
				audio.Stop();
				soundTest = false;
				backButton = true;
				stopMusic();
				level1.Play();
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .28f * Screen.height, 200, .12f * Screen.height), "Level2")) {
				audio.Stop();
				soundTest = false;
				backButton = true;
				stopMusic();
				level2.Play();
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .43f * Screen.height, 200, .12f * Screen.height), "Level3")) {
				audio.Stop();
				soundTest = false;
				backButton = true;
				stopMusic();
				level3.Play();
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .58f * Screen.height, 200, .12f * Screen.height), "Boss1")) {
				audio.Stop();
				soundTest = false;
				backButton = true;
				stopMusic();
				boss1.Play();
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .73f * Screen.height, 200, .12f * Screen.height), "Boss2")) {
				audio.Stop();				
				soundTest = false;
				backButton = true;
				stopMusic();
				boss2.Play();
			}
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .88f * Screen.height, 200, .12f * Screen.height), "Shop")) {
				audio.Stop();
				soundTest = false;
				backButton = true;
				stopMusic();
				shop.Play();
			} 

		}
		
	}
}
