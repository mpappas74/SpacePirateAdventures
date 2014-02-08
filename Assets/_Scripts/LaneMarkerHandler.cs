using UnityEngine;
using System.Collections;

public class LaneMarkerHandler : MonoBehaviour {

	public int laneID;
	public float startZ;

	void Start () {
		transform.position = new Vector3(transform.position.x, transform.position.y, startZ);
		if (laneID == 0) {
			iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath ("lane 1"), "speed", 200, "looptype", iTween.LoopType.loop));
		} else if (laneID == 1) {
			iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath ("lane 2"), "speed", 200, "looptype", iTween.LoopType.loop));
		} else if (laneID == 2) {
			iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath ("lane 3"), "speed", 200, "looptype", iTween.LoopType.loop));
		} else if (laneID == 3) {
			iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath ("lane 4"), "speed", 200, "looptype", iTween.LoopType.loop));
		}
	}
	
}
