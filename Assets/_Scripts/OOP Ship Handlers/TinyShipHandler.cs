using UnityEngine;
using System.Collections;

public class TinyShipHandler : ShipHandler {

	// Use this for initialization
	public override void Start () {
		base.Start();
		firesBolts = true;
		
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}
}
