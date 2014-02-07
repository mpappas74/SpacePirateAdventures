using UnityEngine;
using System.Collections;

public class BackupLaneHandler : MonoBehaviour {

	//STEVENLOOKHERE
	//The below variables handle the movement in the current lanes. If you want to disable all of this logic, just go into the Start()
	//function below and set shouldMoveInLane to false. You can then freely add your own code to handle your lanes and activate it with another bool.
	//************** Move In Lane Logic ********************//
	public bool shouldMoveInLane;	//Should the ship follow a lane or just move directly forward?
	private GameObject upperWall;   //The upper wall of the lane.
	private GameObject lowerWall;	//The lower wall of the lane.
	private bool amInLane = true;	//Whether or not the ship is actually in a lane as far as the code can tell.
	private float deltaUp = 0;		//How much space there is between the ship and the upper wall of the lane.
	private float deltaDown = 0;	//How much space between the ship and lower wall.

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
//If we are moving in a lane, we've got to find the one we are in.
//if (shouldMoveInLane) {
//	DetermineCurrentLane ();
//}

//		//If we are in a lane, we use this logic to track the distance between the ship and the two walls, and keep it vertically in between the walls.
//		if (shouldMoveInLane && amInLane) {
//			float upZ = transform.position.z;
//			RaycastHit hit;
//			if (Physics.Raycast (transform.position, new Vector3 (0, 0, 10), out hit)) {
//				if (hit.transform.gameObject.GetInstanceID () == upperWall.GetInstanceID ()) {
//					deltaUp = hit.point.z - upZ;
//					upZ = hit.point.z;
//				} else {
//					upZ = upZ + deltaUp;
//				}
//			}
//			float downZ = transform.position.z;
//			if (Physics.Raycast (transform.position, new Vector3 (0, 0, -10), out hit)) {
//				if (hit.transform.gameObject.GetInstanceID () == lowerWall.GetInstanceID ()) {
//					deltaDown = hit.point.z - downZ;
//					downZ = hit.point.z;
//				} else {
//					downZ = downZ + deltaDown;
//				}
//			}
//			transform.position += new Vector3 (0.0f, 0.0f, (upZ + downZ) / 2 - transform.position.z);			
//		}

//public void DetermineCurrentLane ()
//{
//	//First, find all lanes that exist, period.
//	GameObject[] allLanes = GameObject.FindGameObjectsWithTag ("Lane");
//	if (allLanes.Length == 0) {
//		amInLane = false;
//	} else {
//		float minDist = 100;
//		float laneWidth = 3;
//		//Next, for each lane, figure out if we are close to any of them.
//		for (int i = 0; i < allLanes.Length; i++) {
//			GameObject curLane = allLanes [i];
//			string name = curLane.transform.name;
//			GameObject upW = GameObject.Find (name + "/UpperWall");
//			float upDistance = 0;
//			RaycastHit hit;
//			//Look up along the z direction and see if you can find this upper wall.
//			if (Physics.Raycast (transform.position, new Vector3 (0, 0, 1), out hit)) {
//				if (hit.transform.gameObject.GetInstanceID () == upW.GetInstanceID ()) {
//					upDistance = hit.distance;
//				} else {
//					upDistance = 1000;
//				}
//			}
//			GameObject lwW = GameObject.Find (name + "/LowerWall");
//			float downDistance = 0;
//			if (Physics.Raycast (transform.position, new Vector3 (0, 0, -1), out hit)) {
//				if (hit.transform.gameObject.GetInstanceID () == lwW.GetInstanceID ()) {
//					downDistance = hit.distance;
//				} else {
//					downDistance = -1000;
//				}
//			}
//			//If we can find both lower and upper walls of a lane correctly oriented around us and not too far away, we say we are in that lane.
//			if (Mathf.Abs (upDistance - downDistance) < minDist) {
//				minDist = upDistance - downDistance;
//				laneWidth = upDistance + downDistance;
//				upperWall = upW;
//				lowerWall = lwW;
//			}
//		}
//		if (minDist == 100) {
//			amInLane = false;
//			Debug.Log ("I'm not in a lane!");
//		}
//		
//		speed = speed * (3f / laneWidth);
//	}
//}

