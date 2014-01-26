using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
//This script is used to allow for player controls, and is added to the PlayerShip gameobject.
{
	public float speed; //How fast the ship should be allowed to move.
	public float tilt;	//As the ship moves left/right, we tilt it so it will look cool. This determines how much.
	public Boundary boundary; //This boundary (see above) allows us to mark where the player is allowed to move.

	public GameObject shot; //The bolt gameobject, so we can instantiate instances of it from the script.
	public Transform shotSpawn; //Where should the bolt be instantiated relative to the player.
	public float fireRate;	//The number of seconds in between each shot.
	
	private float nextFire; //The clock time at which the next shot is allowed.
	
	void Update ()
	{
		//Shot handling for the phone. Each time a touch is begun, a shot is fired, provided it has been long enough since the previous shot.
		for (var i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch (i).phase == TouchPhase.Began && Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
				audio.Play ();
			}
		}

		//Shot handling for the computer. Same as the phone, really.
		if (Input.GetButton("Fire1") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			//The audio that is being called here is the audio file that has been added to the playership object in unity.
			audio.Play ();
		}
	}
	

	void FixedUpdate ()
	{
		//Computer movement handling.
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		//Uncomment the two lines below to be able to move on a phone. (There is probably a way to detect what kind of platform you are on, but for now this is fine.)
		//Note the 2* in the x direction - this is just so you don't have to tilt the phone too much.

		//moveHorizontal = 2*Input.acceleration.x;
		//moveVertical = Input.acceleration.y;

		//We now actually adjust the position of the ship. We change the velocity of the ship, and Unity's physics engine actually moves it.
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed;
		
		rigidbody.position = new Vector3 
			(
				Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
				);
		
		//Note that the rotation is proportional to the speed for improved effect.
		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
	}
}