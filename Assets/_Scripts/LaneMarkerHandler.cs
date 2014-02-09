using UnityEngine;
using System.Collections;

public class LaneMarkerHandler : MonoBehaviour
{

	public int laneID;
	public float startZ;

	void Start ()
	{
		transform.position = new Vector3 (transform.position.x, transform.position.y, startZ);
		
		if (laneID >= 0) {
			int Inlane = laneID + 1;
			string Mylane = Inlane.ToString ();
			iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath ("lane " + Mylane), "speed", 200, "looptype", iTween.LoopType.loop));
		}
	}
	
}
