using UnityEngine;
using System.Collections;

public class ShieldShipHandler : ShipHandler {
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		deploysShield = true;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	public override void Die(bool diedOnscreen = true){
		//Add anything else BEFORE you call base.Die, as base.Die will destroy the gameObject.
		base.Die(diedOnscreen);
	}

	public override void ActivateUpgrades(int UpgradeInt){
		//Do Nothing.
	}
}
