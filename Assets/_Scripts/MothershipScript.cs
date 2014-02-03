using UnityEngine;
using System.Collections;

public class MothershipScript : ShipHandler
{

//The mothershipScript is not too much beyond the shipHandler,
//but the key element is that it updates the string for now.

	private string mainString;
	public GUIText shipHealthText;
	private float wasHealth;

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
			mainString = "Your mothership's ";
		} else {
			mainString = "The enemy's ";
		}
		wasHealth = shipHealth;
		shipHealthText.text = mainString + "health = " + shipHealth.ToString ();
	}
	
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
