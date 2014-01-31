using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
//The camera script handles scrolling the camera by touches.
{

	private Vector3 currentMousePosition; //Only relevant for computer debugging. Tracks mouse position for 'Age of War'-like scrolling.
	public float rightSideCap; //How far (world-wise) can the camera scroll to the right.
	public float leftSideCap;	//How far (world-wise) can the camera scroll to the left.
	public float cameraSpeed;	//How fast should the camera scroll relative to swipe length.
	private Vector3 startingCameraPosition;	//Tracks where the camera is in Y and Z positions, which should never change.
	private float dist;	//This variable will hold the length of a swipe.
	private GameObject levelController;	//Access to the levelController object to determine if we are currently placing a ship.
	private LevelController levelCont;	
	private bool isPlacingShip;	//Just there for convenience. Could equally use script.isPlacingShip.
	private InputHandler input;

	// Use this for initialization
	void Start ()
	{
		levelController = GameObject.Find("LevelController");
		levelCont = levelController.GetComponent<LevelController>();
		input = GameObject.Find("LevelController").GetComponent<InputHandler>();
		//Save the camera's starting position to know its y and z positions, which currently don't change.
		startingCameraPosition = transform.position;
	}


	
	// Update is called once per frame
	void Update ()
	{
		//If we are currently placing a ship, allow for scrolling, but only if the scroll could not be confused
		//for a selection of a place to build the ship. In other words, if the touch is too close to the boxes, refuse to scroll.
		isPlacingShip = levelCont.isPlacingShip;
		if (isPlacingShip) {
			if (input.Moved()) {
					
				//The next three lines just figure out whether the building boxes are yet visible on the screen, and whether the touch was near them.					
				Vector3 pos = input.currentDragPos();
				pos.z = 10.0f;
				pos = Camera.main.ScreenToWorldPoint (pos);

				if (pos.x > 11) {
					//Scroll an x distance proportional to the length of the moving touch, capped on either side.
					dist = input.deltaPos().x;
					transform.position = new Vector3 (Mathf.Clamp (transform.position.x - dist * cameraSpeed, leftSideCap, rightSideCap), startingCameraPosition.y, startingCameraPosition.z);
				}
			}
				
			
		} else {
			//Same as above, but scrolling freely if we are not placing a ship.
			
				if (input.Moved()) {
					dist = input.deltaPos().x;
					transform.position = new Vector3 (Mathf.Clamp (transform.position.x - dist * cameraSpeed, leftSideCap, rightSideCap), startingCameraPosition.y, startingCameraPosition.z);
				
				}

		
		}



		
	}
}