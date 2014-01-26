using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
//The main game controller script handles generation of waves, score evaluation, and gameOvers.
//It also deploys and tracks the objects created through buttons (tinyShips and the like.)
//This script is added to the GameController gameobject.
{

	public GameObject hazard;	//The thing to appear in waves - in this case, asteroids.
	public Vector3 spawnValues;	//Where the asteroids should appear. See below for x-direction handling.
	public int hazardCount;	//How many hazards per wave.
	public float spawnWait;	//How long to wait between each hazard in a wave.
	public float startWait;	//How long before the waves begin.
	public float waveWait;	//How long between waves.
	public GUIText scoreText;	//The GUIText that displays score.
	public GUIText restartText;	//The GUIText that displays instructions to restart after a game over.
	public GUIText gameOverText;	//The GUIText that tells you you have lost.
	private bool gameOver;	//Whether or not the game is over.
	private bool restart;	//Once the game is over, this boolean turns true to denote that you can now restart the game.
	private int score;	//Your score.
	public GameObject testObject;	//The testObject holds the button information currently. Better button handling might remove it.
	private testButton button;	//Access to the button script to check the booleans in it.
	public GameObject player;	//The player gameobject.
	public GameObject wall;		//The wall gameobject to be made after certain button presses.
	public GameObject tinyShip;	//The tinyShip gameobject to be made after certain button presses.
	public GameObject tinySpinnyShip;	//" tinySpinnyShip " " " " " " ".

	void Start ()
	{
		//Actually get access to the button script to have access to booleans denoting whether the button has been pressed.
		button = testObject.GetComponent<testButton> ();
		//We just started, gameOver and restart should be false.
		gameOver = false;
		restart = false;
		//We don't want anything in the restartText or gameOverText boxes for now since that would be in the way of the game.
		restartText.text = "";
		gameOverText.text = "";
		//Set the score to 0 in the script and then update it on screen (see below for UpdateScore routine.)
		score = 0;
		UpdateScore ();
		//Begin spawning waves. See below for SpawnWaves routine.
		StartCoroutine (SpawnWaves ());
	}
	
	void Update ()
	{
		
		//This whole section is button handling. In general, it first checks to see if we are at the main button menu (ships vs walls) or if we are at a sub-menu (the two ship options).
		//It then checks which button has been pressed. If anything has been pressed, it resets all buttons to unpressed and moves to the correct menu.
		//If a button has been pressed to make something, check if the player has enough points. If they do, let them buy it, and initialize the object just in front of the player.
		if (button.main) {
			if (button.pressed) {
				button.pressed = false;
				button.main = false;
			}
			if (button.pressed2) {
				button.pressed2 = false;
				if (score > 50) {
					Vector3 wallPosition = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z + 1);
					//Note that Quaternion.identity is just there to keep from having any rotation. Unfortunately there is no 2-input overloaded version of Instantiate.
					Instantiate (wall, wallPosition, Quaternion.identity);
					score = score - 50;
					UpdateScore ();
				}
			}
		} else {
			if (button.pressed) {
				button.pressed = false;
				button.main = true;
				if (score > 20) {
					Vector3 tinyPosition = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z + 1);
					Instantiate (tinyShip, tinyPosition, Quaternion.identity);
					score = score - 20;
					UpdateScore ();
				}
			}
			if (button.pressed2) {
				button.pressed2 = false;
				button.main = true;
				if (score > 50) {
					Vector3 tinyPosition = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z + 1);
					Instantiate (tinySpinnyShip, tinyPosition, Quaternion.identity);
					score = score - 50;
					UpdateScore ();
				}
			}
		}

		//Restart handling. If we are prepping for a restart, check whether the restart button has been pressed. If it has, reload the current level.
		if (restart) {
			//Restart handling for on-computer.
			if (Input.GetButton ("Fire1")) {
				Application.LoadLevel (Application.loadedLevel);
			}

			//Restart handling for on-phone. (The for loop is mostly unnecessary but helps make sure multiple hits on screen don't throw of the game handling.)
			for (var i = 0; i < Input.touchCount; ++i) {
				//This if statement is basically just checking so we only respond when a touch begins, not when it ends, etc. (All kinds of touch information is stored.)
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					Application.LoadLevel (Application.loadedLevel);
				}
			}
		}
	}
	
	//The coroutine/IEnumetor stuff is basically just useful for the yield option, which allows us to stall the wave generation while letting the rest of the game handle smoothly.
	IEnumerator SpawnWaves ()
	{
		//First, wait until the first wave is meant to start.
		yield return new WaitForSeconds (startWait);

		//Then, we will infinitely send out new waves.
		while (true) {
			//Send out each hazard in the wave one at a time.
			for (int i = 0; i < hazardCount; i++) {
				//Note the Random.range for the x coordinate - this generates asteroids randomly across the x range given rather than at that exact x location.
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				//Now wait until the next hazard is meant to spawn.
				yield return new WaitForSeconds (spawnWait);
			}
			//Now wait until the next wave should happen.
			yield return new WaitForSeconds (waveWait);
			
			//If the game has ended, adjust the restart information, and then break, ending the waves.
			if (gameOver) {
				restartText.text = "Shoot for Restart";
				restart = true;
				break;
			}
		}
	}
	
	//This function is useful for other scripts to call, keeping them from needing direct access to the score variable.
	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}
	
	//Adjust the score text.
	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}
	
	//Adjust the gameOver text. This function is called whenever a gameover condition is met.
	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}