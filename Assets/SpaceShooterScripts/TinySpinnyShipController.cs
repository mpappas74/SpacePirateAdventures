using UnityEngine;
using System.Collections;


public class TinySpinnyShipController : MonoBehaviour
//Very similar to TinyShipController. Only difference is last line.
{
	public float speed;
	
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	
	private float nextFire;
	
	void Update ()
	{
		
		if (Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			audio.Play ();
		}
	}
	
	
	void FixedUpdate ()
	{
		
		Vector3 movement = new Vector3 (0.0f, 0.0f, 1);
		rigidbody.velocity = movement * speed;
		
		//This line just makes the tiny ship be spinning around all the time. 
		rigidbody.angularVelocity = new Vector3 (0.0f, 1.0f, 0.0f);
	}
}