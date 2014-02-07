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
	public bool gameOver; //Whether the game has ended for any reason. Currently the game only ends if you run out of points to build ships with.
	private InputHandler input;	//The input handler.
	public float levelScore;	//The score we have in the current level. Modify in inspector to allow levels where you start with many points.
	public float scoreRegenTime = 1; //How long to wait before increasing score.
	public float scoreRegenMag = 1; //How much score to increase each time.	

	public float[] laneRotations; //How much ship placed into each lane should be rotated. (Note that, if the ship has MoveInLane, it should follow the lane regardless of orientation.)
	public float[] startPositions; //The starting z positions for ships to be built in the lanes which exist.
	private int numLanes;
	public bool playerVictory = false; //Has the player won?
	private ShipHandler mothership; //Need access to the player's and enemy's mothership so that we can tell if we've won.
	private ShipHandler enemyMothership;
	
	void Start ()
	{
		GameControllerScript.Instance.setCurrentLevel (Application.loadedLevel);
		//Be careful here if we change the scene order!!!!!

		//We will need as many placingBoxes as there are lanes, and also the neutral drag ship.
		numLanes = laneRotations.Length;
		placingShipObjects = new GameObject[numLanes + 1];


		button = testObject.GetComponent<ButtonHandler> ();
		//We will check during a build if the click missed the boxes and set it to false if it did.
		clickWasAtBoxes = true;
		gameOver = false;
		//Get access to the input handler.
		input = GameObject.Find ("LevelController").GetComponent<InputHandler> ();

		//...yeah...the coroutine name speaks for itself. 
		StartCoroutine (FixBuggyInput ());
		
		//Slowly heal level score.
		StartCoroutine (GrowLevelScore ());

		mothership = GameObject.Find ("Mothership").GetComponent<ShipHandler> ();
		enemyMothership = GameObject.Find ("EnemyMothership").GetComponent<ShipHandler> ();

	}
	
	IEnumerator GrowLevelScore ()
	{
		while (!button.gameOver) {
			levelScore += scoreRegenMag;
			yield return new WaitForSeconds (scoreRegenTime);
		}
	}

	IEnumerator FixBuggyInput ()
	{
		//OK, basic summary of the below coroutine. We basically check every shield and bomb ship, since
		//they are the ones the player can interact with. We then check if any of them have been clicked.
		//If so, we tell the ship, and the shipHandler does the rest. If not, we have to tell input that we didn't
		//use the click and pass it back to the input handler in case anything else (say, the camera movement script)
		//needs to check if there has been a touch. We also wait .2 seconds between each iteration to save from this taking stupid
		//amounts of processing.

		Vector2 currentClickPos = new Vector2 (0.0f, 0.0f);
		bool foundClickedShip = false;
		while (!gameOver) {
			GameObject[] ShieldShips = GameObject.FindGameObjectsWithTag ("ShieldShip");
			GameObject[] BombShips = GameObject.FindGameObjectsWithTag ("BombShip");
			GameObject[] clickableShips = new GameObject[ShieldShips.Length + BombShips.Length];
			ShieldShips.CopyTo (clickableShips, 0);
			BombShips.CopyTo (clickableShips, ShieldShips.Length);
			

			//if (GameObject.FindWithTag ("BombShip") == null) {
			//	input.setTrigger (false);
			//}
			if (input.Clicked ()) {
				currentClickPos = input.getClickLoc ();
				Vector3 pos = new Vector3 (currentClickPos.x, currentClickPos.y, 10.0f);
				pos = Camera.main.ScreenToWorldPoint (pos);	
				foundClickedShip = false;
				foreach (GameObject ss in clickableShips) {
					if ((pos - ss.transform.position).sqrMagnitude < 9 && ss.layer == LayerMask.NameToLayer ("PlayerShips")) {
						ss.GetComponent<ShipHandler> ().wasClickedOn = true;
						foundClickedShip = true;
						break;
					}
				}
			}
			input.setClicked (false);
			yield return new WaitForSeconds (0.2f);
		}
	}

	
	//This function allows us to build the ship we have chosen in the correct place cleanly while deleting all the other objects in the main script.
	public IEnumerator BuildCurrentShip (GameObject curShip, Vector3 position, float rotation, int laneID)
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

		theShip.GetComponent<ShipHandler> ().laneID = laneID;

		theShip.transform.Rotate (Vector3.up * rotation);
		
		
		

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
		//If the game is over, tell the rest of the game to stop. This is true if we are out of points and have no ships left to earn us more.
		//Notice the order - we only check if we have ships if we are already low on level score. There is still a worst case, but this keeps this from being too big of an efficiency drain.
		if (!button.gameOver && (mothership.isDead || (levelScore < 5 && GameObject.FindWithTag ("TinyShip") == null && GameObject.FindWithTag ("CrazyShip") == null && GameObject.FindWithTag ("LoadingBar") == null && GameObject.FindWithTag ("BombShip") == null && GameObject.FindWithTag ("ShieldShip") == null && GameObject.FindWithTag ("Shield") == null && GameObject.FindWithTag ("StealthShip") == null))) {
			gameOver = true;
			button.gameOver = true;
		} else if (!button.gameOver && (playerVictory || enemyMothership.isDead)) { //Similarly, if playerVictory is true (set by Level_Controller when the waves end
			//or the enemy's mothership is dead, you win!
			GameObject.Find ("GuiTextObjects").transform.Find ("WinText").gameObject.SetActive (true);
			playerVictory = true;
			button.gameOver = true;
		} else {
			
			
			//Next is the button functionality. It will probably help to consider this alongside ButtonHandler if you forget which button is which.
			//Note, however, that this functionality is in Update(), while button declarations are in OnGUI().
			
			//If we are not currently paused...
			if (!button.paused) {
				//If we are not placing a ship and one of the other buttons has been pressed, do as the button commands.
				if (!isPlacingShip) {
					//Buttons 2 and 3 build tinyShips and crazyShips, respectively. 
					if (button.pressed2) {
						button.pressed2 = false;
						currentShip = GameControllerScript.Instance.getTinyShip ();
						currentNeutralShip = GameControllerScript.Instance.getTinyShip ();
						
						//OK, so first, we need the position of the click so we can start dragging.
						//We also need to disable various elements of the neutralShip to keep it from firing or exploding.
						//These buttons are all pretty damn copy-paste: at some point I'll try to make a nicer switch-case out of them.
						Vector3 pos = new Vector3 (input.startPos ().x, input.startPos ().y, 10.0f);
						pos = Camera.main.ScreenToWorldPoint (pos);
						
						placingShipObjects [0] = (GameObject)Instantiate (currentNeutralShip, pos, currentNeutralShip.transform.rotation);
						placingShipObjects [0].SetActive (true);
						placingShipObjects [0].GetComponent<ShipHandler> ().enabled = false;
						placingShipObjects [0].GetComponent<CapsuleCollider> ().enabled = false;
						placingShipObjects [0].transform.Find ("HealthBar").gameObject.GetComponent<MeshRenderer> ().enabled = false;

						isPlacingShip = true;
						//Set canCancelShip to be true so that, on pause, there is an option to cancel the build.
						button.canCancelShip = true;
						mustAddBoxes = true;
						input.setBegan (false);
					}
					if (button.pressed3) {
						button.pressed3 = false;
						currentShip = GameControllerScript.Instance.getCrazyShip ();
						currentNeutralShip = GameControllerScript.Instance.getCrazyShip ();

						Vector3 pos = new Vector3 (input.startPos ().x, input.startPos ().y, 10.0f);
						pos = Camera.main.ScreenToWorldPoint (pos);
						
						placingShipObjects [0] = (GameObject)Instantiate (currentNeutralShip, pos, currentNeutralShip.transform.rotation);
						placingShipObjects [0].SetActive (true);
						placingShipObjects [0].GetComponent<ShipHandler> ().enabled = false;
						placingShipObjects [0].GetComponent<CapsuleCollider> ().enabled = false;
						placingShipObjects [0].transform.Find ("HealthBar").gameObject.GetComponent<MeshRenderer> ().enabled = false;

						isPlacingShip = true;
						button.canCancelShip = true;
						mustAddBoxes = true;
						input.setBegan (false);
					}
					//Button 4 generates bomb ships, and button 5 generates shield ships.
					if (button.pressed4) {
						button.pressed4 = false;
						currentShip = GameControllerScript.Instance.getBombShip ();
						currentNeutralShip = GameControllerScript.Instance.getBombShip ();

						Vector3 pos = new Vector3 (input.startPos ().x, input.startPos ().y, 10.0f);
						pos = Camera.main.ScreenToWorldPoint (pos);
						
						placingShipObjects [0] = (GameObject)Instantiate (currentNeutralShip, pos, currentNeutralShip.transform.rotation);
						placingShipObjects [0].SetActive (true);
						placingShipObjects [0].GetComponent<ShipHandler> ().enabled = false;
						placingShipObjects [0].GetComponent<SphereCollider> ().enabled = false;
						placingShipObjects [0].transform.Find ("HealthBar").gameObject.GetComponent<MeshRenderer> ().enabled = false;
						
						isPlacingShip = true;
						button.canCancelShip = true;
						mustAddBoxes = true;
						input.setBegan (false);
					}
					if (button.pressed5) {
						button.pressed5 = false;
						currentShip = GameControllerScript.Instance.getShieldShip ();
						currentNeutralShip = GameControllerScript.Instance.getShieldShip ();

						Vector3 pos = new Vector3 (input.startPos ().x, input.startPos ().y, 10.0f);
						pos = Camera.main.ScreenToWorldPoint (pos);
						
						placingShipObjects [0] = (GameObject)Instantiate (currentNeutralShip, pos, currentNeutralShip.transform.rotation);
						placingShipObjects [0].SetActive (true);
						placingShipObjects [0].GetComponent<ShipHandler> ().enabled = false;
						placingShipObjects [0].GetComponent<CapsuleCollider> ().enabled = false;
						placingShipObjects [0].transform.Find ("HealthBar").gameObject.GetComponent<MeshRenderer> ().enabled = false;
						
						isPlacingShip = true;
						button.canCancelShip = true;
						mustAddBoxes = true;
						input.setBegan (false);
					}
					//Finally, the bottom button sets main to false, bringing up a sub-menu. So, we could imagine 
					//button6 being the "Heavy Armor" button, and then clicking it brings up two possible heavily armored ships.
					if (button.pressed6) {
						button.pressed6 = false;
						currentShip = GameControllerScript.Instance.getStealthShip ();
						currentNeutralShip = GameControllerScript.Instance.getStealthShip ();
						
						Vector3 pos = new Vector3 (input.startPos ().x, input.startPos ().y, 10.0f);
						pos = Camera.main.ScreenToWorldPoint (pos);
						
						placingShipObjects [0] = (GameObject)Instantiate (currentNeutralShip, pos, currentNeutralShip.transform.rotation);
						placingShipObjects [0].SetActive (true);
						placingShipObjects [0].GetComponent<ShipHandler> ().enabled = false;
						placingShipObjects [0].GetComponent<CapsuleCollider> ().enabled = false;
						placingShipObjects [0].transform.Find ("HealthBar").gameObject.GetComponent<MeshRenderer> ().enabled = false;
						
						isPlacingShip = true;
						button.canCancelShip = true;
						mustAddBoxes = true;
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
				//All pause functionality is handled in ButtonHandler, as none of it needs access to Level info.
			}

//******************************** WE NOW MOVE FROM BUTTON LOGIC TO SHIP-BUILDING LOGIC *************************** 

			if (isPlacingShip) {
			
				if (mustAddBoxes) {
					//We place and orient the boxes according to the future orientation of the built ship. 
					for (int j = 1; j <= numLanes; j++) {
						float rotation = laneRotations [j - 1];
						float zpos = startPositions [j - 1];
						placingShipObjects [j] = (GameObject)Instantiate (GameControllerScript.Instance.getPlacingBox (), new Vector3 (9, -10, zpos), Quaternion.Euler (rotation * Vector3.up));
					}
					mustAddBoxes = false;
				}
				//If we dragged the ship, move it accordingly.
				if (input.Moved ()) {
					Vector2 movePos = input.currentDragPos ();
					placingShipObjects [0].transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (movePos.x, movePos.y, 10.0f));
				}
				
			
				if (input.Ended () || input.Clicked ()) {
					input.setClicked (false);
					Vector3 pos = input.Ended () ? input.endPos () : input.getClickLoc ();
					input.setEnded(false);
					pos.z = 10.0f;
					pos = Camera.main.ScreenToWorldPoint (pos);
					
					//We always assume a click is at the boxes unless shown otherwise.
					clickWasAtBoxes = true;

					//If the touch was too far off to the right or left, you clearly didn't click on the boxes.
					//Additionally, if the touch/click was too far to the left on the screen, you were attempting to hit a button (such as pause).
					//So cancel this as well.
					if (pos.x <= 7 || pos.x > 12) {
						clickWasAtBoxes = false;
					} else if (input.endPos ().x < .25 * Screen.height) {
						clickWasAtBoxes = false;
					}
					
					
					int closestIndex = 0;

					//Now we have to figure out which box you actually released on.
					float currentDiff = Mathf.Abs (pos.z - startPositions [0]);
					for (int j = 1; j < numLanes; j++) {
						if (Mathf.Abs (pos.z - startPositions [j]) < currentDiff) {
							closestIndex = j;
							currentDiff = Mathf.Abs (pos.z - startPositions [j]);
						}
					}
					


					//If we released the neutralShip on a box, try to build it.
					if (clickWasAtBoxes) {

						
						pos.z = startPositions [closestIndex];
						pos.x = 9;
						pos.y = 0;
						float rot = laneRotations [closestIndex];
						
						
						//Check the score. Note that we are only checking it now so that we only decrease it if the player actually builds the ship.
						//We also want the players able to look at ships they don't have the points to build yet.
						//We are no longer trying to place a ship.
						isPlacingShip = false;
						button.canCancelShip = false;
						if (levelScore >= currentShip.GetComponent<ShipHandler> ().cost) {
							levelScore -= currentShip.GetComponent<ShipHandler> ().cost;
							StartCoroutine (BuildCurrentShip (currentShip, pos, rot, closestIndex));
							//Remember, currentShip defaults to placingBox when we are no longer building a ship.
							if (button.shipRepeater) {
								currentShip = currentNeutralShip;//GameControllerScript.Instance.getPlacingBox ();
								placingShipObjects [0].transform.position = new Vector3 (8, 2, 0);
								isPlacingShip = true;
								button.canCancelShip = true;
								input.setEnded (false);
								input.setMoved (false);
							} else {
								currentShip = GameControllerScript.Instance.getPlacingBox ();
								for (int i = 0; i <= numLanes; i++) {
									Destroy (placingShipObjects [i]);
								}
							}
						} else {
							//If we didn't have enough points, make sure GuiTextHandler will know by changing the currentShip object.
							currentShip = GameControllerScript.Instance.getNotEnoughMoneyObject ();
							//Destroy the placingBoxes and the neutralShip.
							for (int i = 0; i <= numLanes; i++) {
								Destroy (placingShipObjects [i]);
							}
						}
					} else { //If we released the ship not at the boxes, cancel the build.
						currentShip = GameControllerScript.Instance.getPlacingBox ();
						for (int i = 0; i <= numLanes; i++) {
							Destroy (placingShipObjects [i]);
						}
						//We are no longer trying to place a ship.
						isPlacingShip = false;
					}
				}
					
			} 
		}
	}
	
	void OnGUI ()
	{
		//We don't need the MainMenu button - buttonHandler already will post that since the game is over.
		//Here, we just add additional buttons that appear when you win/lose.
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
				if (Application.loadedLevel < 3) {
					Application.LoadLevel (Application.loadedLevel + 1);
					GameControllerScript.Instance.setCurrentUnlockedLevel (GameControllerScript.Instance.getCurrentUnlockedLevel () + 1);
				} else {
					Application.LoadLevel ("MainMenu");
				}
			} 
		} else if (gameOver) {
			//if (GUI.Button (new Rect (.5f * Screen.width - 100, .7f * Screen.height, 200, .13f * Screen.height), "Return to Main Menu")) {
			//	Application.LoadLevel ("MainMenu");
			//} 
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .55f * Screen.height, 200, .13f * Screen.height), "Try Again?")) {
				Application.LoadLevel (Application.loadedLevel);
			} 
		}
	}
}
