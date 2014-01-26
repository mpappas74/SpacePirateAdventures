using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {
//This script was in the tutorial but is honestly not really used in the final version. 
//It's really basic anyways - just destroy a certain object after its lifetime (in seconds) has elapsed.

	
	public float lifetime;
	void Start () {
		Destroy(gameObject, lifetime);
	}
	
	
}
