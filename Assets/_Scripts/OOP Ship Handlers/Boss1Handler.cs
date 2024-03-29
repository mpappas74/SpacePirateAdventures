﻿using UnityEngine;
using System.Collections;

public class Boss1Handler : ShipHandler
{
	
	//The mothershipScript is not too much beyond the shipHandler,
	//but the key element is that it updates the string for now.
	
	public GUIText shipHealthText;
	private float wasHealth;
	private float numBoarded;
	
	public override void Start ()
	{
		energyShieldHealth = 0;
		speed = 0.5f;
		cost = 0;
		firesBolts = true;
		deploysShield = false;
		selfDestructs = false;
		scoreValue = 0;
		turnsAroundOnCollision = false;
		boltScale = 2;

		base.Start ();
		wasHealth = shipHealth;
		shipHealthText.text =  shipHealth.ToString () + "/" + numBoarded.ToString();
		healthbar.localScale *= 3/maxHealth;
		maxLength = healthbar.localScale.x;
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
		base.FixedUpdate();
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
	
}
