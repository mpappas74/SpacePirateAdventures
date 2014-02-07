using UnityEngine;
using System.Collections;

public class GuiTextHandler : MonoBehaviour
//Handle all GuiText:: the score and explanation of current ships, both unpaused and paused.
{

	public string explanationText; //The text explaining the currently selected ship.
	public GUIStyle explanationStyle;	//The style for the above text.
	public GUIStyle extraExplanationStyle;	//The style for the above text if the game is paused so we have extra explanation.
	public GUIStyle scoreStyle;	//The style for the score text.
	private GameObject levelController;	//Access to the levelController so we can print the score.
	private LevelController levelCont;
	private GameObject currentBuildingShip; //Whether or not we are currently building a ship, warranting an explanation.
	private bool bonusText;	//Whether or not we are paused and therefore need additional text.
	private GUIStyle labelStyle;	//A style variable to track whether we are using ExplanationStyle or extraExplanationStyle.
	private string gameOverText = "Game Over. Do better next time.";
	public GUIStyle gameOverStyle;

	private ButtonHandler button;
	private string score;
	private bool gameOver;

	// Use this for initialization
	void Start ()
	{
		levelController = GameObject.Find("LevelController");
		levelCont = levelController.GetComponent<LevelController>();
		button = GameObject.Find("EmptyButtonObject").GetComponent<ButtonHandler>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Figure out which ship we are currently building. (If we are building no ship, right now it will return a placement box.)
		currentBuildingShip = levelCont.currentShip;
		//Get the current score.
		score = levelCont.levelScore.ToString();

		//Figure out if we need any extraText, aka if the game has been paused warranting extra text.
		//Currently, due to the click and drag functionality, this is no longer possible physically.
		//However, I expect the explanation text to be replaced eventually by a ship overview menu on the pause screen, so I think it's fine.
		bonusText = (levelCont.isPlacingShip && button.paused);
		
		if (!bonusText) {
			//If no need for extra text, give the explanation according to the ship currently being built.
			labelStyle = explanationStyle;
			if (currentBuildingShip.tag == "TinyShip") {
				explanationText = "This is the basic ship. Cost = 5";
			} else if (currentBuildingShip.tag == "CrazyShip") {
				explanationText = "This ship thinks it is an asteroid. Cost = 5";
			} else if (currentBuildingShip.tag == "NotEnoughMoney") {
				explanationText = "You don't have enough money to build this ship!";
			} else{
				explanationText = "";
			}
		} else{
			//If there is a need for extra text, give the complete explanation.
			labelStyle = extraExplanationStyle;
			if (currentBuildingShip.tag == "TinyShip") {
				explanationText = "This is the basic ship. It has 5 health and fires bullets that do 1 damage. Cost = 5";
			} else if (currentBuildingShip.tag == "CrazyShip") {
				explanationText = "This ship thinks it is an asteroid.It has 2 health and fires bullets that do 1 damage twice as fast as the tiny ship. Cost = 5";
			} else if (currentBuildingShip.tag == "NotEnoughMoney") {
				explanationText = "You don't have enough money to build this ship!";
			}else{
				explanationText = "";
			}
		}

		if(levelCont.gameOver){
			gameOver = true;
		}
	}

	void OnGUI ()
	{
		//Post the texts for explanation and style.
		GUI.Label (new Rect (.25f * Screen.height, .10f * Screen.height, Screen.width - .26f * Screen.height, .2f * Screen.height), explanationText, labelStyle);
		GUI.Label (new Rect (.25f * Screen.height, .02f * Screen.height, .27f * Screen.height, .1f * Screen.height), "Score = " + score, scoreStyle);
		
		//If the game is over, post the gameOver text.
		if(gameOver){
			GUI.Label (new Rect (.5f * Screen.width - 100, .4f * Screen.height, 200, .2f * Screen.height), gameOverText, gameOverStyle);
		}
		
	}
}
