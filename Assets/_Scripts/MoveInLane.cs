using UnityEngine;
using System.Collections;

public class MoveInLane : MonoBehaviour {
	
	private GameObject upperWall;   //The upper wall of the lane.
	private GameObject lowerWall;	//The lower wall of the lane.
	private GameObject testObject;	//The testObject holds the button information currently.
	private ButtonHandler button;	//Access to the button script to check the booleans in it. 
	public float speed;				//The speed at which the ship is moving forward (as determined by its orientation.)
	private bool amInLane = true;	//Whether or not the ship is actually in a lane as far as the code can tell.
	private float deltaUp = 0;		//How much space there is between the ship and the upper wall of the lane.
	private float deltaDown = 0;	//How much space between the ship and lower wall.

	// This entire function is used for exactly one purpose:: to determine whether or not we are in a lane, and 
	//gain access to that lane if we are.
	void Start () {
		testObject = GameObject.Find ("EmptyButtonObject");
		button = testObject.GetComponent<ButtonHandler> ();

		//First, find all lanes that exist, period.
		GameObject[] allLanes = GameObject.FindGameObjectsWithTag ("Lane");
		if (allLanes.Length == 0) {
			amInLane = false;
		} else {
			float minDist = 100;
			//Next, for each lane, figure out if we are close to any of them.
			for (int i = 0; i < allLanes.Length; i++) {
				GameObject curLane = allLanes [i];
				string name = curLane.transform.name;
				GameObject upW = GameObject.Find (name + "/UpperWall");
				float upDistance = 0;
				RaycastHit hit;
				//Look up along the z direction and see if you can find this upper wall.
				if (Physics.Raycast (transform.position, new Vector3(0,0,1), out hit)) {
					if (hit.transform.gameObject.GetInstanceID () == upW.GetInstanceID ()) {
						upDistance = hit.distance;
					} else {
						upDistance = 1000;
					}
				}
				GameObject lwW = GameObject.Find (name + "/LowerWall");
				float downDistance = 0;
				if (Physics.Raycast (transform.position, new Vector3(0,0,-1), out hit)) {
					if (hit.transform.gameObject.GetInstanceID () == lwW.GetInstanceID ()) {
						downDistance = hit.distance;
					} else {
						downDistance = -1000;
					}
				}
				//If we can find both lower and upper walls of a lane correctly oriented around us and not too far away, we say we are in that lane.
				if (Mathf.Abs (upDistance - downDistance) < minDist) {
					minDist = upDistance - downDistance;
					upperWall = upW;
					lowerWall = lwW;
				}
			}
			if (minDist == 100) {
				amInLane = false;
				Debug.Log ("I'm not in a lane!");
			}
		}
	}

	void Update(){
		//If we are in a lane, we use very similar logic to track the distance between the ship and the two walls, and keep it vertically in between the walls.
		if (amInLane) {
			float upZ = transform.position.z;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, new Vector3(0,0,10), out hit)) {
				if (hit.transform.gameObject.GetInstanceID () == upperWall.GetInstanceID ()) {
					deltaUp = hit.point.z - upZ;
					upZ = hit.point.z;
				} else{
					upZ = upZ + deltaUp;
				}
			}
			float downZ = transform.position.z;
			if (Physics.Raycast (transform.position, new Vector3(0,0,-10), out hit)) {
				if (hit.transform.gameObject.GetInstanceID () == lowerWall.GetInstanceID ()) {
					deltaDown = hit.point.z - downZ;
					downZ = hit.point.z;
				} else{
					downZ = downZ + deltaDown;
				}
			}
			transform.position += new Vector3 (0.0f, 0.0f, (upZ + downZ) / 2 - transform.position.z);			
		}
	}
	
	// Meanwhile, we are also moving the ship forward, presuming the game is not paused.
	void FixedUpdate () {
		if (button.paused) {
			rigidbody.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		} else {
			rigidbody.velocity = transform.forward * speed;	
		}
	}
}
