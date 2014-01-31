using UnityEngine;
using System.Collections;

public class ExplosionHandler : ProjectileHandler {
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		isExplosion = true;
		destroysBolts = true;
		destroysShields = true;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}
	
	public override void OnTriggerStay(Collider other){
		base.OnTriggerStay(other);
	}
}
