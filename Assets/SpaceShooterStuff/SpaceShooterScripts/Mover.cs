using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
//This script is used for anything that moves constantly. Here, it is the bolts and the asteroids.
{
	public float speed; //How fast the object should move.
	
	
	void Start ()
	{
		//Just keep moving the object in the desired direction (transform.forward is determined by the orientation of the object in the world view. For this version, it's just in the positive z direction.)
		rigidbody.velocity = transform.forward * speed;
	}
}