using UnityEngine;
using System.Collections;

public class ButtonHandler : MonoBehaviour
	//This script actually handles the buttons onscreen.
	//The script is added to EmptyButtonObject, which just handles the buttons as the appear onscreen.
	//Note that the functionality of the buttons is actually handled in GameController. 
	//This is the case simply because GameController has more access to other variables and objects.
{
	
	//The pressed booleans tell if either button has been pressed.
	//paused, as one might guess, is true if the game is paused and we want all objects to stop moving.
	//canCancelShip is true if we are currently building a ship and want the option to cancel it to appear.
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
	
	private Vector2 scrollPosition = Vector2.zero; //This is necessary for the scroll view that allows us to scroll along the buttons.

	private InputHandler input;

	private string speedString; //This string tracks whether the game could be sped up or down.

	void Start(){
		input = GameObject.Find("LevelController").GetComponent<InputHandler>();	
		
		//The game defaults to the slower speed to give the player more time to react. If they choose to speed it up it will double in speed.
		speedString = "Speed Up Game";
		Time.timeScale = 0.75f;
		Time.fixedDeltaTime *= 0.75f;
	}

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
			if (!paused) {
				//The main, non-paused menu is the basic in-game menu. The top button is the pause button, and the remaining allow you to select various ships.
				if (GUI.Button (new Rect (.03f * Screen.height, .03f * Screen.height, .2f * Screen.height, .13f * Screen.height), "Pause")) {
					pressed1 = true;
				} 
	
				//A scrollview is basically a box that is bigger on the inside than it looks. The first Rect declaration
				//is the size of the view on screen, while the second is the size of the interior of the view.
				//The two empty GUIStyle()'s are there to remove the scroll bar.
				//The scrollview is where the build ship buttons go.
				scrollPosition = GUI.BeginScrollView(new Rect(0.03f*Screen.height, 0.19f*Screen.height, 0.2f*Screen.height, 0.64f*Screen.height), scrollPosition, new Rect(0.0f, 0.0f, 0.13f*Screen.height, 0.8f*Screen.height), new GUIStyle(), new GUIStyle());
				if (input.Moved()) {
					//If we moved, figure out if we did so along the part of the screen where the buttons are, and if the swipe was vertical.
					//If the swipe was vertical and near the buttons, adjust the scrollPosition.
					Vector2 pos = input.currentDragPos();
					Vector2 delta = input.deltaPos();
					if (pos.x < 0.2f*Screen.height && Mathf.Abs(delta.y) > Mathf.Abs(2*delta.x)) {
						//Scroll an x distance proportional to the length of the moving touch, capped on either side.
						scrollPosition.y += input.deltaPos().y;
					} else {
						//This is one of many stupid, hackish fixes to the input handler. I'm working on it.
						input.setMoved(true);
					}
				}
				
				//All of these buttons are now RepeatButtons, which return true at all times that they are pressed down, rather than 
				//just when they are pressed down and fully released. This is useful to allow the click and drag feature, since we
				//don't want to force the player to click a button and then select a thing and drag it.
				if (GUI.RepeatButton (new Rect (0f, 0.0f * Screen.height, .13f * Screen.height, .13f * Screen.height), image1)) {
					pressed2 = true;
				}
				if (GUI.RepeatButton (new Rect  (0f, 0.16f * Screen.height, .13f * Screen.height, .13f * Screen.height), image2)) {
					pressed3 = true;
				} 
				if (GUI.RepeatButton (new Rect  (0f, 0.32f * Screen.height, .13f * Screen.height, .13f * Screen.height), image3)) {
					pressed4 = true;
				} 
				if (GUI.RepeatButton (new Rect  (0f, 0.48f * Screen.height, .13f * Screen.height, .13f * Screen.height), image4)) {
					pressed5 = true;
				} 
				if (GUI.RepeatButton (new Rect  (0f, 0.64f * Screen.height, .2f * Screen.height, .13f * Screen.height), "Thief")) {
					pressed6 = true;
				} 
				GUI.EndScrollView();

			} else {
				//If the game is paused, always have the unpause and main menu buttons. Also, if we are currently building a ship, allow us to cancel it.
				if (canCancelShip && GUI.Button (new Rect (.5f * Screen.width - 100f, .35f * Screen.height, 200f, .08f * Screen.height), "Cancel Ship")) {
					pressed3 = true;
				}
				//We also now how speed up/slow down buttons that allow you to change the speed of the game.
				//Time.timeScale changes how the game processes time in terms of coroutines and the like, while
				//Time.fixedDeltaTime changes how the game processes physics.
				if (GUI.Button (new Rect (.5f * Screen.width - 100f, .45f * Screen.height, 200f, .08f * Screen.height), speedString)) {
					if(speedString == "Speed Up Game"){
						speedString = "Slow Down Game";
						Time.timeScale *= 2f; 
						Time.fixedDeltaTime *= 2f;
					} else {
						speedString = "Speed Up Game";
						Time.timeScale *= 0.5f;
						Time.fixedDeltaTime *= 0.5f;
					}
				} 
				if (GUI.Button (new Rect (.5f * Screen.width - 100f, .55f * Screen.height, 200f, .08f * Screen.height), "Unpause")) {
					pressed1 = true;
				} 
				if (GUI.Button (new Rect (.5f * Screen.width - 100f, .65f * Screen.height, 200f, .08f * Screen.height), "Main Menu")) {
					pressed2 = true;
				} 
			}
		}
	}
}
