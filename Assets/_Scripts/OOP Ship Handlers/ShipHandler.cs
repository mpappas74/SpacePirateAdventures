using UnityEngine;
using System.Collections;

public class ShipHandler : MonoBehaviour
	//Once they have ben built, this script moves ships and has them fire.
{
	public float shipHealth;
	public float enerygShieldHealth;
	public float speed;
	public bool isDead;	//Whether the ship is dead - this allows particular ship controller scripts to handle death differently.
	private float maxHealth; //Maximum health of the ship
	private float maxLength; //Maximum length of the ship's healthbar.
	private Transform healthbar;
	public float cost;	//How much the ship is worth in points after being destroyed.

  //**********************************//
	public bool firesBolts;
	public float fireLag; //How long to wait between shots.
	public GameObject bolt; //Access to the bolt to instantiate shots.
	public Transform shotSpawn; //Where to instantiate shots relative to the ship.
	private float nextFire; //Track the time at which we will next be allowed to fire. 
	//Right as we fire, this is equal to fireLag + Time.time.
	private float timeDif; //A somewhat ugly variable that I added to make pausing work properly. 
	//This keeps one from abusing pausing to make the bolts reload faster.
	public float shotDamage = 1;
	//**********************************//

	private GameObject testObject;	//The testObject holds the button information currently.
	private ButtonHandler button;	//Access to the button script to check the booleans in it. 
	

	public GameObject explosion; //Access to the explosion to instantiate it when the ship dies.
	
	public virtual void Start ()
	{
		testObject = GameObject.Find ("EmptyButtonObject");
		button = testObject.GetComponent<ButtonHandler> ();
		//As we've seen before, getting access to ButtonHandler's booleans to know if we are paused.
		maxHealth = shipHealth;
		healthbar = gameObject.transform.Find ("HealthBar");
		if (healthbar != null) {
			maxLength = healthbar.localScale.x;
		} else {
			maxLength = -1;
		}
		
	}

	public void DecreaseHealth (float healthChange)
	{
		//Decrease health according to the damage done. If the ship is down to 0 health, destroy it,
		//add the score to the player's points, and cause the explosion to occur.
		shipHealth -= healthChange;
		if (shipHealth <= 0) {	
			isDead = true;
		}
		//If the ship has a healthbar attached to it (shields, for instance, currently don't), decrease the length of the healthbar according to damage done.
		if (maxLength > 0) {
			healthbar.localScale = new Vector3 (maxLength * shipHealth / maxHealth, healthbar.localScale.y, healthbar.localScale.z);
		}
	}

	public virtual void Die(){
		Destroy(gameObject);
		Instantiate (explosion, transform.position, transform.rotation);
		audio.Play ();
	}

	public void FireBolts(){
		if (!button.paused) {
			if (Time.time > nextFire) {
				nextFire = Time.time + fireLag;
				GameObject thisBolt = (GameObject)Instantiate (bolt, shotSpawn.position, shotSpawn.rotation);
				BoltMover boltMover = thisBolt.GetComponent<BoltMover> ();
				boltMover.amPlayersBolt = true;
				//if (gameObject.tag == "EnemyShip") {
				//	boltMover.amPlayersBolt = false;
				//}
				boltMover.damageDone = shotDamage;
			}
		} else {
			//At all times, we track how much time is left before we fire again. This allows us to update nextFire in case we pause the game.
			nextFire = Time.time + timeDif;
		}
		timeDif = nextFire - Time.time;
	}
	
	public virtual void Update(){
		if(firesBolts){
			FireBolts();
		}
		if(isDead){
			Die();
		}
	}

}
