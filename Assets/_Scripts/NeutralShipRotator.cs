using UnityEngine;
using System.Collections;

public class NeutralShipRotator : MonoBehaviour {
//A purely aesthetic script to make the demo ships rotate while you are deciding where to place them.
	
	void Start () {
		rigidbody.angularVelocity = new Vector3(0.0f, 1f, 0.0f);
	}
	
}
