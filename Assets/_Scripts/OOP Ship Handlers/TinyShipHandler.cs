using UnityEngine;
using System.Collections;

public class TinyShipHandler : ShipHandler {

	// Use this for initialization
	public override void Start () {
		base.Start();
		firesBolts = true;
		shouldMoveInLane = true;
		
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}
	/*
	public override void FixedUpdate(){
		base.FixedUpdate();
	}
*/
	public override void Die(bool diedOnscreen = true){
		//Add anything else BEFORE you call base.Die, as base.Die will destroy the gameObject.
		base.Die(diedOnscreen);
	}
}
