using UnityEngine;
using System.Collections;

public class MothershipScript : ShipHandler
{

	private bool amPlayers;
	private string mainString;
	public GUIText shipHealthText;
	private float wasHealth;

	// Use this for initialization
	public override void Start ()
	{
		energyShieldHealth = 0;
		speed = 0;
		cost = 0;
		firesBolts = false;
		deploysShield = false;
		selfDestructs = false;
		scoreValue = 0;
		shouldMoveInLane = false;
		turnsAroundOnCollision = false;
		
		base.Start ();

		if (gameObject.layer == LayerMask.NameToLayer ("PlayerShips")) {
			amPlayers = true;
			mainString = "Your mothership's ";
		} else {
			amPlayers = false;
			mainString = "The enemy's ";
		}
		wasHealth = shipHealth;
		shipHealthText.text = mainString + "health = " + shipHealth.ToString ();
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		if (wasHealth != shipHealth) {
			wasHealth = shipHealth;
			if (shipHealth <= 0) {
				shipHealth = 0;
			}
			shipHealthText.text = mainString + "health = " + shipHealth.ToString ();
		}
		base.Update ();
	}

	public override void OnTriggerEnter (Collider other)
	{
		base.OnTriggerEnter (other);
	}
	
	public override void Die (bool diedOnscreen = true)
	{
		base.Die (diedOnscreen);
	}

	public override void FixedUpdate(){
		//Do nothing.
	}

}
