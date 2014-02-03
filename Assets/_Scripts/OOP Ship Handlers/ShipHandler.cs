using UnityEngine;
using System.Collections;

public class ShipHandler : MonoBehaviour
	//Once they have ben built, this script moves ships and has them fire.
{
	//************** Common Properties of All Ships ********************//
	public float shipHealth;
	public float energyShieldHealth;
	public float speed;
	public bool isDead;	//Whether the ship is dead - this allows particular ship controller scripts to handle death differently.
	private float maxHealth; //Maximum health of the ship
	private float maxLength; //Maximum length of the ship's healthbar.
	private Transform healthbar;
	public float cost;	//How much the ship is worth in points after being destroyed.
	private InputHandler input;

	//************** Bolt Firing Logic ********************//
	public bool firesBolts;
	public float fireLag; //How long to wait between shots.
	public GameObject bolt; //Access to the bolt to instantiate shots.
	public Transform shotSpawn; //Where to instantiate shots relative to the ship.
	private float nextFire; //Track the time at which we will next be allowed to fire. 
	//Right as we fire, this is equal to fireLag + Time.time.
	private float timeDif; //A somewhat ugly variable that I added to make pausing work properly. 
	//This keeps one from abusing pausing to make the bolts reload faster.
	public float shotDamage = 1;
	public float boltSurvivalTime = -1;

	//************** Shield Deployment Logic ********************//
	public bool wasClickedOn;
	public bool wasReleasedOn;
	public bool deploysShield;
	public GameObject shield; //Access to the shield to instantiate them.
	private Vector2 currentClickPos;

	//************** SelfDestruction Logic ********************//
	public bool selfDestructs;
	public GameObject blastZone; //The physical blast zone in which things take damage from the bomb.

	//************** PauseButton Access Variables ********************//
	private GameObject testObject;	//The testObject holds the button information currently.
	private ButtonHandler button;	//Access to the button script to check the booleans in it. 
	
	//************** Death Logic ********************//
	public GameObject explosion; //Access to the explosion to instantiate it when the ship dies.
	public float scoreValue;
	
	//************** Move In Lane Logic ********************//
	public bool shouldMoveInLane;
	private GameObject upperWall;   //The upper wall of the lane.
	private GameObject lowerWall;	//The lower wall of the lane.
	private bool amInLane = true;	//Whether or not the ship is actually in a lane as far as the code can tell.
	private float deltaUp = 0;		//How much space there is between the ship and the upper wall of the lane.
	private float deltaDown = 0;	//How much space between the ship and lower wall.
		
	//************** Turn Around On Collision Logic ********************//
	public bool turnsAroundOnCollision;
	public float collectedResources = 0;
	
	//************** MiniMap Logic ********************//
	private GameObject enemyDot = (GameObject)Resources.Load ("EnemyshipDot");
	private GameObject playerDot = (GameObject)Resources.Load ("PlayershipDot");
	private GameObject myDot;
	private Vector3 worldScale;
	private Vector3 mapShift;

	//***************************************** Virtual Methods ******************************************************//
 
	public virtual void Start ()
	{
		testObject = GameObject.Find ("EmptyButtonObject");
		button = testObject.GetComponent<ButtonHandler> ();
		//As we've seen before, getting access to ButtonHandler's booleans to know if we are paused.
		maxHealth = shipHealth;
		healthbar = gameObject.transform.Find ("HealthBar");
		healthbar.localScale *= maxHealth / 3;
		if (healthbar != null) {
			maxLength = healthbar.localScale.x;
		} else {
			maxLength = -1;
		}
		if (shouldMoveInLane) {
			DetermineCurrentLane ();
		}
		input = GameObject.Find ("LevelController").GetComponent<InputHandler> ();

		worldScale = GameObject.Find ("Background").transform.localScale;
		worldScale = new Vector3 (1 / worldScale.x, 1, 1 / worldScale.z);
		GameObject miniMap = GameObject.Find ("MiniMap");
		myDot = (gameObject.layer == LayerMask.NameToLayer("EnemyShips")) ? enemyDot : playerDot;
		myDot = (GameObject)Instantiate (myDot, myDot.transform.position, myDot.transform.rotation);
		myDot.transform.parent = miniMap.transform;
		mapShift = new Vector3 (0.5f, -1.2f, 0.0f);
		myDot.transform.localPosition = Vector3.Scale (transform.position, worldScale) - mapShift;

	}

	public virtual void OnTriggerEnter (Collider other)
	{
		if (turnsAroundOnCollision) {
			if (other.gameObject.layer == LayerMask.NameToLayer("EnemyShips")) {
				transform.Rotate (new Vector3 (0.0f, 180f, 0.0f));
				collectedResources += 5;
				foreach (Transform child in transform) {
					if (child.name == "HealthBar") {
						child.localPosition -= new Vector3 (2 * child.localPosition.x, 0.0f, 0.0f);
					}
				}
			} else if (other.tag == "Player") {
				GameObject.Find ("LevelController").GetComponent<LevelController> ().levelScore += collectedResources;
				collectedResources = 0;
				transform.Rotate (new Vector3 (0.0f, 180f, 0.0f));
				foreach (Transform child in transform) {
					if (child.name == "HealthBar") {
						child.localPosition -= new Vector3 (2 * child.localPosition.x, 0.0f, 0.0f);
					}
				}
			}
		} else if(other.gameObject.layer == LayerMask.NameToLayer("EnemyShips") && gameObject.layer != LayerMask.NameToLayer("EnemyShips")) {
			float damage = Mathf.Min(shipHealth, other.gameObject.GetComponent<ShipHandler>().shipHealth);
			DecreaseHealth(damage);
			other.gameObject.GetComponent<ShipHandler>().DecreaseHealth(damage);
		}
	}

	public virtual void Update ()
	{
		myDot.transform.localPosition = Vector3.Scale (transform.position, worldScale) - mapShift;
		if (firesBolts) {
			FireBolts ();
		}
		if (deploysShield) {
			ShieldDeploy ();
		}
		if (selfDestructs) {
			if (input.isTrigger ()) {
				Explode ();
			} else if (isDead) {
				Explode ();
			}
		} else if (isDead) {
			Die ();
		}
		//If we are in a lane, we use very similar logic to track the distance between the ship and the two walls, and keep it vertically in between the walls.
		if (amInLane) {
			float upZ = transform.position.z;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, new Vector3 (0, 0, 10), out hit)) {
				if (hit.transform.gameObject.GetInstanceID () == upperWall.GetInstanceID ()) {
					deltaUp = hit.point.z - upZ;
					upZ = hit.point.z;
				} else {
					upZ = upZ + deltaUp;
				}
			}
			float downZ = transform.position.z;
			if (Physics.Raycast (transform.position, new Vector3 (0, 0, -10), out hit)) {
				if (hit.transform.gameObject.GetInstanceID () == lowerWall.GetInstanceID ()) {
					deltaDown = hit.point.z - downZ;
					downZ = hit.point.z;
				} else {
					downZ = downZ + deltaDown;
				}
			}
			transform.position += new Vector3 (0.0f, 0.0f, (upZ + downZ) / 2 - transform.position.z);			
		}
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


	// Meanwhile, we are also moving the ship forward, presuming the game is not paused.
	public virtual void FixedUpdate ()
	{
		if (button.paused) {
			rigidbody.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		} else {
			rigidbody.velocity = transform.forward * speed;	
		}
	}
	
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
		if (!button.paused) {
			if (Time.time > nextFire) {
				nextFire = Time.time + fireLag;
				GameObject thisBolt = (GameObject)Instantiate (bolt, shotSpawn.position, shotSpawn.rotation);
				ProjectileHandler boltMover = thisBolt.GetComponent<ProjectileHandler> ();
				thisBolt.layer = LayerMask.NameToLayer("PlayerAttacks");
				if (gameObject.layer == LayerMask.NameToLayer("EnemyShips")) {
					thisBolt.layer = LayerMask.NameToLayer("EnemyAttacks");
				}
				boltMover.damageDone = shotDamage;
				if (boltSurvivalTime > 0) {
					boltMover.survivalTime = boltSurvivalTime;
				}
				boltMover.speed += speed;
			}
		} else {
			//At all times, we track how much time is left before we fire again. This allows us to update nextFire in case we pause the game.
			nextFire = Time.time + timeDif;
		}
		timeDif = nextFire - Time.time;
	}

	public void DetermineCurrentLane ()
	{
		//First, find all lanes that exist, period.
		GameObject[] allLanes = GameObject.FindGameObjectsWithTag ("Lane");
		if (allLanes.Length == 0) {
			amInLane = false;
		} else {
			float minDist = 100;
			float laneWidth = 3;
			//Next, for each lane, figure out if we are close to any of them.
			for (int i = 0; i < allLanes.Length; i++) {
				GameObject curLane = allLanes [i];
				string name = curLane.transform.name;
				GameObject upW = GameObject.Find (name + "/UpperWall");
				float upDistance = 0;
				RaycastHit hit;
				//Look up along the z direction and see if you can find this upper wall.
				if (Physics.Raycast (transform.position, new Vector3 (0, 0, 1), out hit)) {
					if (hit.transform.gameObject.GetInstanceID () == upW.GetInstanceID ()) {
						upDistance = hit.distance;
					} else {
						upDistance = 1000;
					}
				}
				GameObject lwW = GameObject.Find (name + "/LowerWall");
				float downDistance = 0;
				if (Physics.Raycast (transform.position, new Vector3 (0, 0, -1), out hit)) {
					if (hit.transform.gameObject.GetInstanceID () == lwW.GetInstanceID ()) {
						downDistance = hit.distance;
					} else {
						downDistance = -1000;
					}
				}
				//If we can find both lower and upper walls of a lane correctly oriented around us and not too far away, we say we are in that lane.
				if (Mathf.Abs (upDistance - downDistance) < minDist) {
					minDist = upDistance - downDistance;
					laneWidth = upDistance + downDistance;
					upperWall = upW;
					lowerWall = lwW;
				}
			}
			if (minDist == 100) {
				amInLane = false;
				Debug.Log ("I'm not in a lane!");
			}
		
			speed = speed * (3f / laneWidth);
		}
	}

	public void ShieldDeploy ()
	{
		if (!button.paused) {
			if (wasClickedOn && wasReleasedOn) {
				GameObject theShield = (GameObject)Instantiate (shield, shotSpawn.position, shotSpawn.rotation);
				theShield.layer = LayerMask.NameToLayer("PlayerAttacks");
				if(gameObject.layer == LayerMask.NameToLayer("EnemyShips")){
					theShield.layer = LayerMask.NameToLayer("EnemyAttacks");
				}
				wasClickedOn = false;
				wasReleasedOn = false;
				Destroy (gameObject);
			}
		}
	}

	//If the bomb explodes, remove it, and replace it with an explosion and blastzone.
	public void Explode ()
	{
		Instantiate (explosion, transform.position, transform.rotation);
		Instantiate (blastZone, transform.position, transform.rotation);
		audio.Play ();
		Destroy (gameObject);
	}

}
