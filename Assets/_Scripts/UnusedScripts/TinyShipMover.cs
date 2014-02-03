using UnityEngine;
using System.Collections;

public class TinyShipMover : MonoBehaviour
//Once they have ben built, this script moves ships and has them fire.
{
	public float fireLag; //How long to wait between shots.
	public GameObject bolt; //Access to the bolt to instantiate shots.
	public Transform shotSpawn; //Where to instantiate shots relative to the ship.
	private float nextFire; //Track the time at which we will next be allowed to fire. 
	//Right as we fire, this is equal to fireLag + Time.time.
	private GameObject testObject;	//The testObject holds the button information currently.
	private ButtonHandler button;	//Access to the button script to check the booleans in it. 
	
	private float timeDif; //A somewhat ugly variable that I added to make pausing work properly. 
	//This keeps one from abusing pausing to make the bolts reload faster.

	public GameObject explosion; //Access to the explosion to instantiate it when the ship dies.

	public float tinyShipDamage = 1;
	public float crazyShipDamage = 1;

	void Start ()
	{
		testObject = GameObject.Find ("EmptyButtonObject");
		button = testObject.GetComponent<ButtonHandler> ();
		//As we've seen before, getting access to ButtonHandler's booleans to know if we are paused.
		
	}
	
	void Update ()
	{
		if (GetComponent<HealthTracker> ().isDead) {
			Destroy (gameObject);
			Instantiate (explosion, transform.position, transform.rotation);
			audio.Play ();
		}
		//If we are not paused, just keep shooting separated by the correct amount of time.
		if (!button.paused) {
			if (Time.time > nextFire) {
				nextFire = Time.time + fireLag;
				GameObject thisBolt = (GameObject)Instantiate (bolt, shotSpawn.position, shotSpawn.rotation);
				BoltMover boltMover = thisBolt.GetComponent<BoltMover> ();
				boltMover.amPlayersBolt = true;
				if (gameObject.tag == "EnemyShip") {
					boltMover.amPlayersBolt = false;
				}
				if (gameObject.tag == "TinyShip") {
					boltMover.damageDone = tinyShipDamage;
				} else if (gameObject.tag == "CrazyShip") {
					boltMover.damageDone = crazyShipDamage;
				} else {
					boltMover.damageDone = 1;
				}
			}
		} else {
			//At all times, we track how much time is left before we fire again. This allows us to update nextFire in case we pause the game.
			nextFire = Time.time + timeDif;
		}
		timeDif = nextFire - Time.time;
		
	}

}
