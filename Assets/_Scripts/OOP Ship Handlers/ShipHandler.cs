using UnityEngine;
using System.Collections;

public class ShipHandler : MonoBehaviour
	//Once they have been built, this script moves ships and has them fire.
{
	//************** Common Properties of All Ships ********************//
	public float shipHealth; //basic health
	public float energyShieldHealth; //Energy shield health. Not yet valuable in any way.
	public float speed; //Speed. Speaks for itself.
	public bool isDead;	//Whether the ship is dead - this allows particular ship controller scripts to handle death differently.
	public float maxHealth; //Maximum health of the ship
	public float maxLength; //Maximum length of the ship's healthbar.
	public Transform healthbar; //The physical healthbar above each ship.
	public float cost;	//How much the ship costs to build.

	//************** Bolt Firing Logic ********************//
	public bool firesBolts;	//Does this ship fire bolts?
	public float fireLag; //How long to wait between shots.
	public GameObject bolt; //Access to the bolt to instantiate shots.
	public Transform shotSpawn; //Where to instantiate shots relative to the ship.
	private float nextFire; //Track the time at which we will next be allowed to fire. 
	//Right as we fire, this is equal to fireLag + Time.time.
	private float timeDif; //A somewhat ugly variable that I added to make pausing work properly. 
	//This keeps one from abusing pausing to make the bolts reload faster.
	public float shotDamage = 1; //How much each bolt should do.
	public float boltSurvivalTime = -1; //How long each bolt should survive. Negative values mean they last forever.

	//************** Input Logic ********************//
	public bool wasClickedOn;	//Was this ship just clicked on?
	
	//************** Shield Deployment Logic ********************//
	public bool deploysShield; //Does this ship deploy shields?
	public GameObject shield; //Access to the shield to instantiate them.

	//************** SelfDestruction Logic ********************//
	public bool selfDestructs;	//Does this ship blow itself up?
	public GameObject blastZone; //The physical blast zone in which things take damage from the bomb.
	
	//************** Death Logic ********************//
	public GameObject explosion; //Access to the explosion to instantiate it when the ship dies.
	public float scoreValue;	//How many points this ship is worth upon destruction.
	
	//************** Move In Lane Logic ********************//
	public int laneID = -1;
		
	//************** Turn Around On Collision Logic ********************//
	public bool turnsAroundOnCollision;	//Should the ship turn around on collisions or fight the colliding ship?
	public float collectedResources = 0;	//How many resources the thief ship has collected.
	
	//************** MiniMap Logic ********************//
	private GameObject myDot; //The actual dot representing this particular ship.
	private Vector3 worldScale; //A scaling vector representing the relative size of the miniMap to the actual arena.
	private Vector3 mapShift; //The shift off of the center of the map to orient the dots correctly.
	int nextPoint=1;


	//***************************************** Virtual Methods ******************************************************//

	void Tween (UnityEngine.Vector3[] path) {

		UnityEngine.Vector3 toPoint = path[nextPoint];
		Vector3.Distance(toPoint, transform.position);
		iTween.MoveTo (gameObject, iTween.Hash ("position", toPoint, "time", Vector3.Distance(toPoint, transform.position)/speed, "movetopath", false, "oncomplete","complete",
		"completeparams", path, "easetype", iTween.EaseType.linear));
	
	}
	void complete(UnityEngine.Vector3[] path) {
		nextPoint++;
		if (nextPoint < path.Length) Tween(path);
	}
 
	public virtual void Start ()
	{
		speed = 10f;
		//Set up the healthbar. If we have one, set its initial length based on the max health of the ship.
		maxHealth = shipHealth;
		healthbar = gameObject.transform.Find ("HealthBar");
		if (healthbar != null) {
			healthbar.localScale *= maxHealth / 3;
			maxLength = healthbar.localScale.x;
		} else {
		}

		if (laneID >= 0) {
				int Inlane=laneID+1;
				string Mylane=Inlane.ToString();
			UnityEngine.Vector3[] the_path= iTweenPath.GetPath("lane"+Mylane);
			Tween (the_path);
			   // iTween.MoveTo (gameObject, iTween.Hash ("path", iTweenPath.GetPath ("lane " + Mylane), "time", speed, "movetopath", false));
				}




		//Set up the miniMap. First calculate the scaling factor for the miniMap : world ratio.
		worldScale = GameObject.Find ("Background").transform.localScale;
		worldScale = new Vector3 (1 / worldScale.x, 1, 1 / worldScale.z);

		//Then instantiate a dot for this ship as a child of the map and orient it correctly.
		GameObject miniMap = GameObject.Find ("MiniMap");
		myDot = (gameObject.layer == LayerMask.NameToLayer ("EnemyShips")) ? (GameObject)Resources.Load ("EnemyshipDot") : (GameObject)Resources.Load ("PlayershipDot");
		myDot = (GameObject)Instantiate (myDot, myDot.transform.position, myDot.transform.rotation);
		myDot.transform.parent = miniMap.transform;
		mapShift = new Vector3 (0.5f, -1.2f, 0.0f);
		myDot.transform.localPosition = Vector3.Scale (transform.position, worldScale) - mapShift;

	}

	public virtual void OnTriggerEnter (Collider other)
	{
		//If we turn around on collisions, check if we collided with a ship. If we did, turn around, and be sure to reposition the health bar so it is still above the ship.
		//Notice that we can just check if the ship was on the enemy or player ship layer, since, if, say, this ship was a player ship,
		//it can't collide with the player layer, so this script will only truly react to colliding with an enemy ship, which is what we want.
		if (turnsAroundOnCollision) {
			if (other.gameObject.layer == LayerMask.NameToLayer ("EnemyShips") || other.gameObject.layer == LayerMask.NameToLayer ("PlayerShips")) {
				
				transform.Rotate (new Vector3 (0.0f, 180f, 0.0f));
				collectedResources += 5;
				if (other.tag == "Enemy" || other.tag == "Player") {
					collectedResources += 10;
				}
				foreach (Transform child in transform) {
					if (child.name == "HealthBar") {
						child.localPosition -= new Vector3 (2 * child.localPosition.x, 0.0f, 0.0f);
					}
				}
			}
		} else if (other.gameObject.layer == LayerMask.NameToLayer ("EnemyShips") && gameObject.layer != LayerMask.NameToLayer ("EnemyShips")) {
			
			//If we don't turn around on collisions, then collide with the other ship. The above isItAnEnemyAndI'mNot logic is to keep from double subtracting accidentally.
			//A collision results in both ships taking damage equal to the weaker one's health.

			float damage = Mathf.Min (shipHealth, other.gameObject.GetComponent<ShipHandler> ().shipHealth);
			DecreaseHealth (damage);
			other.gameObject.GetComponent<ShipHandler> ().DecreaseHealth (damage);
		}
	}

	public virtual void Update ()
	{
		//Update your dot position.
	//	myDot.transform.localPosition = Vector3.Scale (transform.position, worldScale) - mapShift;

		//Run a bunch of boolean checks based on what kind of behavior this ship is meant to exhibit, and then run the corresponding function.
		if (firesBolts) {
			FireBolts ();
		}
		if (deploysShield) {
			ShieldDeploy ();
		}
		if (turnsAroundOnCollision && transform.position.x < 0) {
			
			GameObject.Find ("LevelController").GetComponent<LevelController> ().levelScore += collectedResources;
			collectedResources = 0;
			transform.Rotate (new Vector3 (0.0f, 180f, 0.0f));
			foreach (Transform child in transform) {
				if (child.name == "HealthBar") {
					child.localPosition -= new Vector3 (2 * child.localPosition.x, 0.0f, 0.0f);
				}
			}
		}
		if (selfDestructs) {
			if (wasClickedOn) {
				wasClickedOn = false;
				Explode ();
			} else if (isDead) {
				Explode ();
			}
		} else if (isDead) {
			Die ();
		}

		//Flip the ship and healthbar around if the ship is turning backwards.
		if (turnsAroundOnCollision) {
			if (transform.position.x >= 80) {
				
				transform.Rotate (new Vector3 (0.0f, 180f, 0.0f));
				foreach (Transform child in transform) {
					if (child.name == "HealthBar") {
						child.localPosition -= new Vector3 (2 * child.localPosition.x, 0.0f, 0.0f);
					}
				}
			}
		}
	}

	// Meanwhile, we are also moving the ship forward.
	public virtual void FixedUpdate ()
	{
		if (laneID < 0) {
			rigidbody.velocity = transform.forward * speed;	
		}
	}
	
	//If a ship dies, update the level score, explode, and then destroy both the ship and its minimap dot.
	public virtual void Die (bool diedOnscreen = true)
	{
		if (diedOnscreen) {
			LevelController lc = GameObject.Find ("LevelController").GetComponent<LevelController> ();
			lc.levelScore += scoreValue;
			Instantiate (explosion, transform.position, transform.rotation);
			audio.Play ();
		}
		Destroy (myDot);
		Destroy (gameObject);
	}


	//***************************************** Standard Methods ******************************************************//

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

	public void FireBolts ()
	{
		//Fire bolts and place them on a layer according to whether we are an enemy or player ship.
		if (Time.time > nextFire) {
			nextFire = Time.time + fireLag;
			GameObject thisBolt = (GameObject)Instantiate (bolt, shotSpawn.position, shotSpawn.rotation);
			ProjectileHandler boltMover = thisBolt.GetComponent<ProjectileHandler> ();
			thisBolt.layer = LayerMask.NameToLayer ("PlayerAttacks");
			if (gameObject.layer == LayerMask.NameToLayer ("EnemyShips")) {
				thisBolt.layer = LayerMask.NameToLayer ("EnemyAttacks");
			}
			boltMover.damageDone = shotDamage;
			if (boltSurvivalTime > 0) {
				boltMover.survivalTime = boltSurvivalTime;
			}
			//Give the bolt a relative speed boost from the ship.
			boltMover.speed += speed;
		}
	
		timeDif = nextFire - Time.time;
	}

	//Replace the ship with a shield at the same place.
	public void ShieldDeploy ()
	{
		if (wasClickedOn) {
			GameObject theShield = (GameObject)Instantiate (shield, shotSpawn.position, shotSpawn.rotation);
			theShield.layer = LayerMask.NameToLayer ("PlayerAttacks");
			if (gameObject.layer == LayerMask.NameToLayer ("EnemyShips")) {
				theShield.layer = LayerMask.NameToLayer ("EnemyAttacks");
			}
			wasClickedOn = false;
			Destroy (myDot);
			Destroy (gameObject);
		}
	}

	//If the bomb explodes, remove it, and replace it with an explosion and blastzone.
	public void Explode ()
	{
		Instantiate (blastZone, transform.position, transform.rotation);
		Die ();
	}

}
