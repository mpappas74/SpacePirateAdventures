using UnityEngine;
using System.Collections;

public class BoltHandler : ProjectileHandler {

	// Use this for initialization
	public override void Start () {
			base.Start();
			doesSingleShotDamage = true;
	}
	
	// Update is called once per frame
	public override void Update () {
			base.Update();
	}

	public override void OnTriggerStay(Collider other){
		base.OnTriggerStay(other);
	}
}
