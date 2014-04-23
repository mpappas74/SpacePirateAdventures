using UnityEngine;
using System.Collections;

public class FighterShipHandler : ShipHandler {
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		firesBolts = true;
		
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}
	
	public override void Die(bool diedOnscreen = true){
		//Add anything else BEFORE you call base.Die, as base.Die will destroy the gameObject.
		base.Die(diedOnscreen);
	}
	
	/*private string[] fighterShipMenuStrings = {
		"Exploding Bullets",
		"Ramming Shield",
		"Health Increase"
	};*/


	public override void ActivateUpgrades(int UpgradeInt){
		if(UpgradeInt % 10 == 1){
			UpgradeInt = UpgradeInt - 1;
			explodingBolt = true;
		}
		UpgradeInt = UpgradeInt/10;
		if(UpgradeInt % 10 == 1){
			UpgradeInt = UpgradeInt - 1;
			hasRamShield = true;
		}
		UpgradeInt = UpgradeInt/10;
		if(UpgradeInt % 10 == 1){
			UpgradeInt = UpgradeInt - 1;
			shipHealth = 8;
		}
	}
}
