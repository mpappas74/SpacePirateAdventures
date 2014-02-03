using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
//The levelController class is always the same. It handles the constant factors in each level:: buttons that instantiate ships,
//gameOver conditions, and building new ships. It gets the ship prefabs from gameController, in which they should have been
//updated to reflect any upgrades. 

//Lane information must be added in the inspector (see laneRotations and startPositions).
{
	
	public GameObject testObject;	//The testObject holds the button information currently. 
	private ButtonHandler button;	//Access to the button script to check the booleans in it.
	public bool isPlacingShip;	//Are we currently trying to place a ship somewhere?
	public GameObject currentShip;	//The ship that we are currently considering building. 
	public GameObject currentNeutralShip;
	//If we are not currently building a ship, this object defaults to the placingBox.
	private GameObject[] placingShipObjects;	//The placingBoxes and neutral ship objects that we instantiate.
	//This allows us to delete them once a ship is actually being built.
	private Vector3 initialClickPos; //Tracks the location of the initial touch/click when building a ship.
	private bool mustAddBoxes;		//Makes sure we only add placing boxes once per build.
	private bool clickWasAtBoxes;	//Determines whether we actually touched a box, or just somewhere else on the screen.
	//This object exists to be passed to the GuiTextHandler so it can tell you you don't have enough money.
	public bool extraText;	//Whether we need extra explanatory text.
	public bool gameOver; //Whether the game has ended for any reason. Currently the game only ends if you run out of points to build ships with.
	private InputHandler input;	//The input handler.
	private bool buttonJustPressedThisUpdate; //Makes sure button clicks aren't counted as multiple touches.
	public float levelScore;
	public float[] laneRotations; //How much ship placed into each lane should be rotated. (Note that, if the ship has MoveInLane, it should follow the lane regardless of orientation.)
	public float[] startPositions; //The starting z positions for ships to be built in the lanes which exist.
	public bool playerVictory = false; //Has the player won?
	private ShipHandler mothership;
	private ShipHandler enemyMothership;
	
	void Start ()
	{
		GameControllerScript.Instance.setCurrentLevel (Application.loadedLevel);
		//Be careful here if we change the scene order!!!!!

		placingShipObjects = new GameObject[laneRotations.Length + 1];
		button = testObject.GetComponent<ButtonHandler> ();
		//We will check during a build if the click missed the boxes and set it to false if it did.
		clickWasAtBoxes = true;
		gameOver = false;
		//Get access to the input handler.
		input = GameObject.Find ("LevelController").GetComponent<InputHandler> ();
		StartCoroutine (FixBuggyInput ());
		mothership = GameObject.Find ("Mothership").GetComponent<ShipHandler> ();
		enemyMothership = GameObject.Find("EnemyMothership").GetComponent<ShipHandler>();
	}
	
	IEnumerator FixBuggyInput ()
	{
		Vector2 currentClickPos = new Vector2 (0.0f, 0.0f);
		bool foundClickedShip = false;
		while (!gameOver) {
			GameObject[] ShieldShips = GameObject.FindGameObjectsWithTag ("ShieldShip");
			if (GameObject.FindWithTag ("BombShip") == null) {
				input.setTrigger (false);
			}
			if (input.Began ()) {
				currentClickPos = input.startPos ();
				Vector3 pos = new Vector3 (currentClickPos.x, currentClickPos.y, 10.0f);
				pos = Camera.main.ScreenToWorldPoint (pos);	
				foundClickedShip = false;
				foreach (GameObject ss in ShieldShips) {
					if ((pos - ss.transform.position).sqrMagnitude < 4) {
						ss.GetComponent<ShipHandler> ().wasClickedOn = true;
						foundClickedShip = true;
						break;
					}
				}
				if (!foundClickedShip) {
					input.setBegan (true);
				}
			}
			if (foundClickedShip && input.Ended ()) {
				foundClickedShip = false;
				if ((input.endPos () - currentClickPos).sqrMagnitude < 4) {
					Vector3 pos = new Vector3 (currentClickPos.x, currentClickPos.y, 10.0f);
					pos = Camera.main.ScreenToWorldPoint (pos);
					foreach (GameObject ss in ShieldShips) {
						if ((pos - ss.transform.position).sqrMagnitude < 4) {
							ss.GetComponent<ShipHandler> ().wasReleasedOn = true;
							foundClickedShip = true;
							break;
						}
					}
				}
				if (!foundClickedShip) {
					input.setEnded (true);
				}
			}
			

			yield return new WaitForSeconds (0.2f);
		}
	}

	
	//This function allows us to build the ship we have chosen in the correct place cleanly while deleting all the other objects in the main script.
	public IEnumerator BuildCurrentShip (GameObject curShip, Vector3 position, float rotation)
	{
		//Keep one placingBox visible to show that the ship is being built. Hold it for one minute, then replace it with a ship.
		//We rotate both the placingBox and the ship by the movementAngle about the y axis.
		GameObject block = (GameObject)Instantiate (GameControllerScript.Instance.getLoadingBar (), new Vector3 (9, -10, position.z), Quaternion.Euler (new Vector3 (0.0f, rotation, 0.0f)));
		float initialHeight = block.transform.localScale.z;
		for (int i = 0; i < 5; i++) {
			yield return new WaitForSeconds (0.2f);
			block.transform.localScale = new Vector3 (block.transform.localScale.x, block.transform.localScale.y, initialHeight * (5f - i) / 5f);
		}
		Destroy (block);
		GameObject theShip = (GameObject)Instantiate (curShip, position, curShip.transform.rotation);
		theShip.transform.Rotate (Vector3.up * rotation);
		theShip.SetActive (true);
		//An attempt at rotating the particle systems along with the ship (aka the jet propulsion). So far unsuccessful.
		//		ParticleSystem[] particleSystems = theShip.GetComponentsInChildren<ParticleSystem> ();
		//		foreach (ParticleSystem p in particleSystems) {
		//			p.startRotation -= movementAngle;
		//		}
		
	}
	
	void Update ()
	{
		if (buttonJustPressedThisUpdate && input.Began ()) {
			input.setBegan (true);
			buttonJustPressedThisUpdate = false;
		}
		//If the game is over, tell the rest of the game to stop. This is true if we are out of points and have no ships left to earn us more.
		if (mothership.isDead || (levelScore < 5 && GameObject.FindWithTag ("TinyShip") == null && GameObject.FindWithTag ("CrazyShip") == null && GameObject.FindWithTag ("LoadingBar") == null && GameObject.FindWithTag ("BombShip") == null && GameObject.FindWithTag ("ShieldShip") == null && GameObject.FindWithTag ("Shield") == null && GameObject.FindWithTag ("StealthShip") == null)) {
			gameOver = true;
			button.gameOver = true;
		} else if (playerVictory || enemyMothership.isDead) {
			button.gameOver = true;
		} else {
			
			
			//Next is the button functionality. It will probably help to consider this alongside ButtonHandler if you forget which button is which.
			//Note, however, that this functionality is in Update(), while button declarations are in OnGUI().
			
			//If we are not currently paused...
			if (!button.paused) {
				//If we click the pause button, now we are paused.
				if (button.pressed1) {
					button.pressed1 = false;
					button.paused = true;
					buttonJustPressedThisUpdate = true;
					input.setBegan (false);
				}
				//Otherwise, if we are not placing a ship and one of the other buttons has been pressed, do as the button commands.
				if (!isPlacingShip) {
					//Buttons 2 and 3 build tinyShips and crazyShips, respectively. 
					if (button.pressed2) {
						button.pressed2 = false;
						currentShip = GameControllerScript.Instance.getTinyShip ();
						currentNeutralShip = GameControllerScript.Instance.getTinyShip ();
						
						//OK, only going to say this once. The below ugly list of .enabled bools being changed is to make a Neutral form of the ship we are building. 
						//This is much more logical than having separate neutralShips as different prefabs in my opinion, despite the ugly code.
						placingShipObjects [0] = (GameObject)Instantiate (currentNeutralShip, new Vector3 (6, -3, 0), Quaternion.identity);
						placingShipObjects [0].SetActive (true);
						placingShipObjects [0].GetComponent<NeutralShipRotator> ().enabled = true;
						placingShipObjects [0].GetComponent<ShipHandler> ().enabled = false;
						placingShipObjects [0].GetComponent<CapsuleCollider> ().enabled = false;
						placingShipObjects [0].transform.Find ("HealthBar").gameObject.GetComponent<MeshRenderer> ().enabled = false;

						isPlacingShip = true;
						//Set canCancelShip to be true so that, on pause, there is an option to cancel the build.
						button.canCancelShip = true;
						mustAddBoxes = true;
						buttonJustPressedThisUpdate = true;
						input.setBegan (false);
					}
					if (button.pressed3) {
						button.pressed3 = false;
						currentShip = GameControllerScript.Instance.getCrazyShip ();
						currentNeutralShip = GameControllerScript.Instance.getCrazyShip ();

						placingShipObjects [0] = (GameObject)Instantiate (currentNeutralShip, new Vector3 (6, -3, 0), Quaternion.identity);
						placingShipObjects [0].SetActive (true);
						placingShipObjects [0].GetComponent<NeutralShipRotator> ().enabled = true;
						placingShipObjects [0].GetComponent<ShipHandler> ().enabled = false;
						placingShipObjects [0].GetComponent<CapsuleCollider> ().enabled = false;
						placingShipObjects [0].transform.Find ("HealthBar").gameObject.GetComponent<MeshRenderer> ().enabled = false;

						isPlacingShip = true;
						button.canCancelShip = true;
						mustAddBoxes = true;
						buttonJustPressedThisUpdate = true;
						input.setBegan (false);
					}
					//Button 4 generates bomb ships, and button 5 generates shield ships.
					if (button.pressed4) {
						button.pressed4 = false;
						currentShip = GameControllerScript.Instance.getBombShip ();
						currentNeutralShip = GameControllerScript.Instance.getBombShip ();

						placingShipObjects [0] = (GameObject)Instantiate (currentNeutralShip, new Vector3 (6, -3, 0), Quaternion.identity);
						placingShipObjects [0].GetComponent<NeutralShipRotator> ().enabled = true;
						placingShipObjects [0].GetComponent<ShipHandler> ().enabled = false;
						placingShipObjects [0].GetComponent<SphereCollider> ().enabled = false;
						placingShipObjects [0].transform.Find ("HealthBar").gameObject.GetComponent<MeshRenderer> ().enabled = false;
						
						isPlacingShip = true;
						button.canCancelShip = true;
						mustAddBoxes = true;
						buttonJustPressedThisUpdate = true;
						input.setBegan (false);
					}
					if (button.pressed5) {
						button.pressed5 = false;
						currentShip = GameControllerScript.Instance.getShieldShip ();
						currentNeutralShip = GameControllerScript.Instance.getShieldShip ();

						placingShipObjects [0] = (GameObject)Instantiate (currentNeutralShip, new Vector3 (6, -3, 0), Quaternion.identity);
						placingShipObjects [0].SetActive (true);
						placingShipObjects [0].GetComponent<NeutralShipRotator> ().enabled = true;
						placingShipObjects [0].GetComponent<ShipHandler> ().enabled = false;
						placingShipObjects [0].GetComponent<CapsuleCollider> ().enabled = false;
						placingShipObjects [0].transform.Find ("HealthBar").gameObject.GetComponent<MeshRenderer> ().enabled = false;
						
						isPlacingShip = true;
						button.canCancelShip = true;
						mustAddBoxes = true;
						buttonJustPressedThisUpdate = true;
						input.setBegan (false);
					}
					//Finally, the bottom button sets main to false, bringing up a sub-menu. So, we could imagine 
					//button6 being the "Heavy Armor" button, and then clicking it brings up two possible heavily armored ships.
					if (button.pressed6) {
						button.pressed6 = false;
						currentShip = GameControllerScript.Instance.getStealthShip ();
						currentNeutralShip = GameControllerScript.Instance.getStealthShip ();
						
						placingShipObjects [0] = (GameObject)Instantiate (currentNeutralShip, new Vector3 (6, -3, 0), Quaternion.identity);
						placingShipObjects [0].SetActive (true);
						placingShipObjects [0].GetComponent<NeutralShipRotator> ().enabled = true;
						placingShipObjects [0].GetComponent<ShipHandler> ().enabled = false;
						placingShipObjects [0].GetComponent<CapsuleCollider> ().enabled = false;
						placingShipObjects [0].transform.Find ("HealthBar").gameObject.GetComponent<MeshRenderer> ().enabled = false;
						
						isPlacingShip = true;
						button.canCancelShip = true;
						mustAddBoxes = true;
						buttonJustPressedThisUpdate = true;
						input.setBegan (false);
					}
				} else {
					//This else corresponds to the time when we are placing a ship. We currently throw away all button clicks during this time. Otherwise, they are all saved up and executed in sequence upon the current ship being built.
					button.pressed1 = false;
					button.pressed2 = false;
					button.pressed3 = false;
					button.pressed4 = false;
					button.pressed5 = false;
					button.pressed6 = false;
				}
			} else {
				//This else corresponds to the paused boolean, so this is the pause menu.
				
				//If we wanted to cancel a currently built ship, do so.
				if (button.pressed3) {
					button.pressed3 = false;
					button.paused = false;
					extraText = false;
					for (int i = 0; i <= laneRotations.Length; i++) {
						Destroy (placingShipObjects [i]);
					}
					currentShip = GameControllerScript.Instance.getPlacingBox ();
					isPlacingShip = false;
					button.canCancelShip = false;
					//Reset the input values. This is due to poorly designed sequencing of touches in my input script and may later be unnecessary.
					//For now, though, it prevents the gameController from saving the touch position/angle information and immediately jumping on that the next time we try to build a ship.
					input.Start ();
					
				}
				//Otherwise, if we click Unpause, unpause the game.
				if (button.pressed1) {
					//OK, this is really dumb, but if you hit Unpause over the swiping box, the input handler thinks it is a swipe.
					//This is because it cannot currently separate button presses from normal touches.
					//We currently fix this by once again resetting the input values so that it doesn't think we've made a touch. 
					button.pressed1 = false;
					button.paused = false;
					extraText = false;
					input.Start ();
					
				}
				//If we click return to Menu, we will do so. Eventually we'll want to be able to save information before returning to menu.
				if (button.pressed2) {
					Application.LoadLevel ("MainMenu");
				}
			}
			//If we are currently placing a ship, enter this logic.
			if (isPlacingShip) {
				//The first time we enter this upon trying to place a ship, add the placingBoxes. Don't let it happen again, though, or you will have as many boxes as there were frames before you choose where to build your ship.
				if (mustAddBoxes) {
					//We place and orient the boxes according to the future orientation of the built ship. 
					for (int j = 1; j <= laneRotations.Length; j++) {
						float rotation = laneRotations [j - 1];
						float zpos = startPositions [j - 1];
						placingShipObjects [j] = (GameObject)Instantiate (GameControllerScript.Instance.getPlacingBox (), new Vector3 (9, -10, zpos), Quaternion.Euler (rotation * Vector3.up));
					}
					mustAddBoxes = false;
				}
				if (!buttonJustPressedThisUpdate) {
					//If a touch has happened, check where it was.
					if (input.Began ()) {
						Vector3 pos = input.startPos ();
						//Vector3 pos = Input.mousePosition;
						pos.z = 10.0f;
						pos = Camera.main.ScreenToWorldPoint (pos);
						if (pos.x >= 8 && pos.x <= 12) {
							initialClickPos = pos;
						} else{
							input.setBegan(true);
						}
					} else if (input.Ended ()) {	
						//Convert the touch into a world coordinate.
						Vector3 pos = input.endPos ();
						//Vector3 pos = Input.mousePosition;
						pos.z = 10.0f;
						pos = Camera.main.ScreenToWorldPoint (pos);
						if (Vector3.Dot (pos - initialClickPos, pos - initialClickPos) > 10) {
							clickWasAtBoxes = false;
						}
						//If the touch was too far off to the right or left, you clearly didn't click on the boxes.
						//Additionally, if the touch/click was too far to the left on the screen, you were attempting to hit a button (such as pause).
						//So cancel this as well.
						if (pos.x <= 4 || pos.x > 12) {
							clickWasAtBoxes = false;
						} else if (input.startPos ().x < .25 * Screen.height) {
							clickWasAtBoxes = false;
						}
						//If the click was in this range, you clicked on the neutralShip, which means we need to pause the game
						//and give extra explanation about the ship you are currently building.
						if (pos.x < 8 && pos.x > 4) {
							button.paused = true;
							clickWasAtBoxes = false;
							extraText = true;
						}

						//****************DUMB INPUT FIX DO NOT TOUCH FOR NOW
						if(pos.x > 12){
							input.setEnded(true);
						}



						int closestIndex = 0;
						float currentDiff = Mathf.Abs (pos.z - startPositions [0]);
						for (int j = 1; j < laneRotations.Length; j++) {
							if (Mathf.Abs (pos.z - startPositions [j]) < currentDiff) {
								closestIndex = j;
								currentDiff = Mathf.Abs (pos.z - startPositions [j]);
							}
						}
					
						//Otherwise, you clicked the boxes. So figure out which box you clicked on.
						if (clickWasAtBoxes) {
							pos = initialClickPos;
							pos.z = startPositions [closestIndex];
							float rot = laneRotations [closestIndex];
						
						
						
							//Check the score. Note that we are only checking it now so that we only decrease it if the player actually builds the ship.
							//We also want the players able to look at ships they don't have the points to build yet.
							button.canCancelShip = false;
							if (levelScore >= 5) {
								levelScore -= 5;
								StartCoroutine (BuildCurrentShip (currentShip, pos, rot));
								//Remember, currentShip defaults to placingBox when we are no longer building a ship.
								currentShip = GameControllerScript.Instance.getPlacingBox ();
							} else {
								//If we didn't have enough points, make sure GuiTextHandler will know by changing the currentShip object.
								currentShip = GameControllerScript.Instance.getNotEnoughMoneyObject ();
							}
							//Destroy the placingBoxes and the neutralShip.
							for (int i = 0; i <= laneRotations.Length; i++) {
								Destroy (placingShipObjects [i]);
							}
							//We are no longer trying to place a ship.
							isPlacingShip = false;
						}
						//We always assume a click is at the boxes unless shown otherwise.
						clickWasAtBoxes = true;
					}
				}
				
			} 
		}
	}
	
	void OnGUI ()
	{
		if (playerVictory) {
			//if (GUI.Button (new Rect (.5f * Screen.width - 100, .7f * Screen.height, 200, .13f * Screen.height), "Return to Main Menu")) {
			//	Application.LoadLevel ("MainMenu");
			//} 
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .55f * Screen.height, 200, .13f * Screen.height), "Upgrades")) {
				Application.LoadLevel ("MainMenu");
				//Application.LoadLevel("UpgradeScene");
			} 
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .4f * Screen.height, 200, .13f * Screen.height), "Next Level")) {
				//Be careful here if we change the scene order!!!!
				if (Application.loadedLevel < 2) {
					Application.LoadLevel (Application.loadedLevel + 1);
					GameControllerScript.Instance.setCurrentUnlockedLevel (GameControllerScript.Instance.getCurrentUnlockedLevel () + 1);
				} else {
					Application.LoadLevel ("MainMenu");
				}
			} 
		}
	}
}
