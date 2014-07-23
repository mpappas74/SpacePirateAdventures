using UnityEngine;
using System.Collections;

public class MothershipScript : ShipHandler
{

//The mothershipScript is not too much beyond the shipHandler,
//but the key element is that it updates the string for now.

	public GUIText shipHealthText;
	private float wasHealth;
	private int numBoarded;

	public override void Start ()
	{
		energyShieldHealth = 0;
		speed = 0;
		cost = 0;
		firesBolts = false;
		deploysShield = false;
		selfDestructs = false;
		scoreValue = 0;
		turnsAroundOnCollision = false;
	
		if(GameControllerScript.Instance.mothershipHealth && gameObject.tag == "Player"){
			shipHealth += 15;
		}

		base.Start ();
		wasHealth = shipHealth;
		shipHealthText.text =  shipHealth.ToString () + "/" + numBoarded.ToString();
		healthbar.localScale *= 3/maxHealth;
		maxLength = healthbar.localScale.x;

		StartCoroutine("takeDamage");
	}
	
	public override void Update ()
	{
		if (wasHealth != shipHealth) {
			wasHealth = shipHealth;
			if (shipHealth <= 0) {
				shipHealth = 0;
			}
			shipHealth = Mathf.Round(shipHealth * 100) / 100;
			shipHealthText.text = shipHealth.ToString () + "/" + numBoarded.ToString();
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

	public override void board(){
		numBoarded = numBoarded + 1;
	}

	IEnumerator takeDamage(){
		float damageTaken = 0;
		while(true){
			yield return new WaitForSeconds(1);
			damageTaken = 0.1f*numBoarded;
			DecreaseHealth(damageTaken);
		}
	}

	public override bool SpecialAvailable(){
		return numBoarded==0;
	}

	public override void deboard(){
		numBoarded = 0;
	}

	public override void ActivateUpgrades(int UpgradeInt){
		//Do Nothing.
	}

}
