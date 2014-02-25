using UnityEngine;
using System.Collections;


public class TinyShipController : MonoBehaviour
//This script controls the Tiny Ships that are summoned, and goes on the Tiny Ship in the Prefab folder.
{
	public float speed; //How fast it moves.
	public GameObject shot; //The bolt object.
	public Transform shotSpawn; //Where to make the bolt relative to the ship.
	public float fireRate; //How fast to fire.
	
	private float nextFire; //When firing is next allowed.
	
	void Update ()
	{
		//Simple time check to keep from firing too frequently.
		if (Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			audio.Play ();
		}
	}
	
	
	void FixedUpdate ()
	{
		//Keeps the ship moving constantly forward.
		Vector3 movement = new Vector3 (0.0f, 0.0f, 1);
		rigidbody.velocity = movement * speed;
		
	}
}