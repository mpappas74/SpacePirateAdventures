using UnityEngine;
using System.Collections;

public class ShieldHandler : ProjectileHandler {
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		takesDamage = true;
		givesDamageBack = true;
		damageFromShip = 10;
		damageBack = 10000;
		projectileHealth = 20;
	}
	
	// Update is called once per frame
	public override void FixedUpdate () {
		base.FixedUpdate();
	}
	
	public override void OnTriggerStay(Collider other){
		base.OnTriggerStay(other);
	}
}
