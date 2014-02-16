using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {

	public GUIStyle style;
	private int exp = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnGUI () {
		if(GUI.Button(new Rect(Screen.width * 0.9f - 25, Screen.height*0.5f, 50, 30), "Next?")){
			exp += 1;
		}
		switch (exp) {
		case 0:
			GUI.Box(new Rect(Screen.width * 0.3f - 80, Screen.height*0.5f, 160, 100), "This is your mothership. If it dies, it's game over.", style);
			break;
		case 1:
			GUI.Box(new Rect(Screen.width * 0.7f - 80, Screen.height*0.8f, 160, 100), "The red bar is your enemy's health, and the blue is yours.", style);
			break;
		case 2:
			GUI.Box(new Rect(Screen.width * 0.3f - 80, Screen.height*0.5f, 160, 100), "These buttons will allow you to build ships.", style);
			break;
		case 3:
			GUI.Box(new Rect(Screen.width * 0.35f - 80, Screen.height*0.7f, 160, 100), "You can scroll the level or see your ships in this mini map.", style);
			break;
		case 4:
			GUI.Box(new Rect(Screen.width * 0.5f - 100, Screen.height*0.5f, 200, 100), "You use your special bar to fire manual shots by clicking on screen, or unleash your special from the button menu.", style);
			break;
		case 5:
			GUI.Box(new Rect(Screen.width * 0.5f - 100, Screen.height*0.5f, 200, 100), "Survive all enemy waves or destroy the enemy mothership to win! Pillage as much loot as you can! Good luck!", style);
			break;
		case 6:
			Application.LoadLevel("MainMenu");
			break;
		}
	}
}
