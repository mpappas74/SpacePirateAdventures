using UnityEngine;
using System.Collections;

public class BoltHandler : ProjectileHandler {

	// Use this for initialization
	public override void Start () {
		fadesAway = true;
		doesSingleShotDamage = true;
		base.Start();
	}
	
	// Update is called once per frame
	public override void FixedUpdate () {
			base.FixedUpdate();
	}

	public override void OnTriggerStay(Collider other){
		base.OnTriggerStay(other);
	}
}
