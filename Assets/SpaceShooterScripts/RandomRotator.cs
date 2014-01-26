using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {
//This script just chooses random angular velocities for each asteroid when they spawn.
//The script is added to the asteroid objects.
	
	public float tumble;
	
	// Use this for initialization
	void Start () {
		rigidbody.angularVelocity = Random.insideUnitSphere * tumble;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
