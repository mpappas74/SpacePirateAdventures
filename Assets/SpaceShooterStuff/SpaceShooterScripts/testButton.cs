using UnityEngine;
using System.Collections;

public class testButton : MonoBehaviour
//This script actually handles the buttons onscreen.
//The script is added to EmptyButtonObject, which just handles the buttons as the appear onscreen.
{

	//The boolean main determines whether we are at the basic button menu (ships vs walls) or the sub-menu (tiny ship vs tiny spinny ship.)
	//The pressed booleans tell if either button has been pressed.
	public bool main;
	public bool pressed;
	public bool pressed2;

	//The images are just the images that go on each button.
	public Texture image;
	public Texture image2;
	public Texture image3;
	public Texture image4;

	void OnGUI ()
	{
		//Generate the buttons in locations based on screen size to keep a consistent positioning.
		//Note that the buttons are actually declared IN the if statement - this seems to work and was how the stack overload example I found worked, but dunno if it is more or less efficient this way.
		//The declaration of the button is also a function which returns a boolean about whether the button has been pressed, so this seems like the best way to declare it.
		if (main) {
			if (GUI.Button (new Rect (.03f * Screen.height, .9f * Screen.height, .07f * Screen.height, .07f * Screen.height), image)) {
				pressed = true;
			} 
			if (GUI.Button (new Rect (.13f * Screen.height, .9f * Screen.height, .07f * Screen.height, .07f * Screen.height), image2)) {
				pressed2 = true;
			} 
		} else {
			if (GUI.Button (new Rect (.03f * Screen.height, .9f * Screen.height, .07f * Screen.height, .07f * Screen.height), image3)) {
				pressed = true;
			} 
			if (GUI.Button (new Rect (.13f * Screen.height, .9f * Screen.height, .07f * Screen.height, .07f * Screen.height), image4)) {
				pressed2 = true;
			} 
		}
	}
}
