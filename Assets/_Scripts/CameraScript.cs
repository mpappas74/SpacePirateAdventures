using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
//The camera script handles scrolling the camera by touches.
{
	public float rightSideCap; //How far (world-wise) can the camera scroll to the right.
	public float leftSideCap;	//How far (world-wise) can the camera scroll to the left.
	public float cameraSpeed;	//How fast should the camera scroll relative to swipe length.
	private Vector3 startingCameraPosition;	//Tracks where the camera is in Y and Z positions, which should never change.
	private float dist;	//This variable will hold the length of a swipe.
	private GameObject levelController;	//Access to the levelController object to determine if we are currently placing a ship.
	private LevelController levelCont;
	private bool isPlacingShip;	//Just there for convenience. Could equally use script.isPlacingShip.
	private InputHandler input; //Access to input handler.

	public GameObject miniMap; //Access to miniMap gameObject to keep it always in the correct camera view.
	public GameObject healthbar;
	public GameObject enemyHealthbar;
	public GameObject specialLoadingBar;

	void Start ()
	{
		levelController = GameObject.Find ("LevelController");
		levelCont = levelController.GetComponent<LevelController> ();
		input = GameObject.Find ("LevelController").GetComponent<InputHandler> ();
		//Save the camera's starting position to know its y and z positions, which currently don't change.
		startingCameraPosition = transform.position;
	}

	void Update ()
	{
		dist = 0;
		//First, we must check if we are placing a ship. 
		isPlacingShip = levelCont.isPlacingShip;
		if (isPlacingShip) {
			if (input.Moved ()) {
				//Find the location of the touch in the world.	
				Vector3 pos = input.currentDragPos ();
				pos.z = 10.0f;
				pos = Camera.main.ScreenToWorldPoint (pos);

				//If the touch was sufficiently far to the right, accept it as a camera swipe.
				if (pos.x > 12) {
					//Scroll an x distance proportional to the length of the moving touch, capped on either side.
					dist = input.deltaPos ().x;
					//Notice in the Clamp expression below that I am ONLY allowing the player to scroll left while they have a ship selected.
					//This is because the only thing they should be trying to do is reach the boxes to build a ship.
					dist = Mathf.Clamp (transform.position.x - dist * cameraSpeed, leftSideCap, transform.position.x) - transform.position.x;
					transform.position = new Vector3 (transform.position.x + dist, startingCameraPosition.y, startingCameraPosition.z);
				} 
			}
		} else {
			//Same as above, but scrolling freely if we are not placing a ship.
			if (input.Moved ()) {
				Vector3 pos = input.currentDragPos ();
				pos.z = 10.0f;
				pos = Camera.main.ScreenToWorldPoint (pos);
				
				if (pos.x > 9) {
					dist = input.deltaPos ().x;
					dist = Mathf.Clamp (transform.position.x - dist * cameraSpeed, leftSideCap, rightSideCap) - transform.position.x;
					transform.position = new Vector3 (transform.position.x + dist, startingCameraPosition.y, startingCameraPosition.z);
					input.setMoved(false);
				} 
			}
		}

		//Simply adjust the position of the miniMap according to how much the camera has shifted.
		if (dist != 0) {
			miniMap.transform.position = new Vector3 (miniMap.transform.position.x + dist, miniMap.transform.position.y, miniMap.transform.position.z); 
			if(healthbar != null){
				healthbar.transform.position = new Vector3(healthbar.transform.position.x + dist, healthbar.transform.position.y, healthbar.transform.position.z);
			}
			if(enemyHealthbar != null){
				enemyHealthbar.transform.position = new Vector3(enemyHealthbar.transform.position.x + dist, enemyHealthbar.transform.position.y, enemyHealthbar.transform.position.z);
			}
			if(specialLoadingBar != null){
				specialLoadingBar.transform.position = new Vector3(specialLoadingBar.transform.position.x + dist, specialLoadingBar.transform.position.y, specialLoadingBar.transform.position.z);
			}
		}
		
	}
}