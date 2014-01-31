using UnityEngine;
using System.Collections;

public class ButtonHandler : MonoBehaviour
	//This script actually handles the buttons onscreen.
	//The script is added to EmptyButtonObject, which just handles the buttons as the appear onscreen.
	//Note that the functionality of the buttons is actually handled in GameController. 
	//This is the case simply because GameController has more access to other variables and objects.
{
	
	//The boolean main determines whether we are at the basic button menu (ships vs walls) or the sub-menu (tiny ship vs tiny spinny ship.)
	//The pressed booleans tell if either button has been pressed.
	//paused, as one might guess, is true if the game is paused and we want all objects to stop moving.
	//canCancelShip is true if we are currently building a ship and want the option to cancel it to appear.
	public bool main;
	public bool pressed1;
	public bool pressed2;
	public bool pressed3;
	public bool pressed4;
	public bool pressed5;
	public bool pressed6;
	public bool paused;
	public bool canCancelShip;
	public bool gameOver;
	
	//The images are just the images that go on each button.;
	public Texture image1;
	public Texture image2;
	public Texture image3;
	public Texture image4;
	public Texture image5;
	
	void OnGUI ()
	{
		//Generate the buttons in locations based on screen size to keep a consistent positioning.
		//Note that the buttons are actually declared IN the if statement, since they are methods themselves that return booleans.
		
		//If the game is over, offer a button to return to the main menu.
		if (gameOver) {
			paused = true;
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .7f * Screen.height, 200, .13f * Screen.height), "Return to Main Menu")) {
				Application.LoadLevel ("MainMenu");
			} 
		} else {
			if (main && !paused) {
				//The main, non-paused menu is the basic in-game menu. The top button is the pause button, and the remaining allow you to select various ships.
				if (GUI.Button (new Rect (.03f * Screen.height, .03f * Screen.height, .2f * Screen.height, .13f * Screen.height), "Pause")) {
					pressed1 = true;
				} 
				if (GUI.Button (new Rect (.03f * Screen.height, .19f * Screen.height, .13f * Screen.height, .13f * Screen.height), image1)) {
					pressed2 = true;
				} 
				if (GUI.Button (new Rect (.03f * Screen.height, .35f * Screen.height, .13f * Screen.height, .13f * Screen.height), image2)) {
					pressed3 = true;
				} 
				if (GUI.Button (new Rect (.03f * Screen.height, .51f * Screen.height, .13f * Screen.height, .13f * Screen.height), image3)) {
					pressed4 = true;
				} 
				if (GUI.Button (new Rect (.03f * Screen.height, .67f * Screen.height, .13f * Screen.height, .13f * Screen.height), image4)) {
					pressed5 = true;
				} 
				if (GUI.Button (new Rect (.03f * Screen.height, .83f * Screen.height, .16f * Screen.height, .13f * Screen.height), "More")) {
					pressed6 = true;
				} 
			} else if (!paused) {
				//The non-main, non-paused menu is an example of a sub-menu if there are multiple types of ships that you choose between after hitting a button on the main menu.
				if (GUI.Button (new Rect (.03f * Screen.height, .03f * Screen.height, .2f * Screen.height, .13f * Screen.height), "Pause")) {
					pressed1 = true;
				} 
				if (GUI.Button (new Rect (.03f * Screen.height, .19f * Screen.height, .2f * Screen.height, .13f * Screen.height), "Thief")) {
					pressed2 = true;
				} 
				if (GUI.Button (new Rect (.03f * Screen.height, .35f * Screen.height, .2f * Screen.height, .13f * Screen.height), "Back")) {
					pressed3 = true;
				} 
			} else {
				//If the game is paused, always have the unpause and main menu buttons. Also, if we are currently building a ship, allow us to cancel it.
				if (canCancelShip && GUI.Button (new Rect (.5f * Screen.width - 100f, .35f * Screen.height, 200f, .08f * Screen.height), "Cancel Ship")) {
					pressed3 = true;
				}
				if (GUI.Button (new Rect (.5f * Screen.width - 100f, .45f * Screen.height, 200f, .08f * Screen.height), "Unpause")) {
					pressed1 = true;
				} 
				if (GUI.Button (new Rect (.5f * Screen.width - 100f, .55f * Screen.height, 200f, .08f * Screen.height), "Main Menu")) {
					pressed2 = true;
				} 
			}
		}
	}
}
