using UnityEngine;
using System.Collections;

public class MotherShipController : MonoBehaviour
//This script is added to the MotherShip gameobject.
{

	public float health; //How many hits the mothership can take.
	public GameController gameController;	//The gameController.
	public GUIText motherText;	//The health text for the mothership.


	void Update ()
	{
		//If health ever dips below 0, first of all, just leave it at 0, and then call a game over.
		if (health <= 0) {
			health = 0;
			gameController.GameOver ();
		}
		motherText.text = health.ToString ();
		
			
	}

	//If the mothership is hit by something, this is called, since the mothership is a trigger.
	void OnTriggerEnter (Collider other)
	{
		//If the thing that hit the mothership is an enemy weapon (the tag used for the asteroids), decrease health and destroy the asteroids.
		if (other.tag == "EnemyWeapon") {
			health = health - 1;
		}
		Destroy(other.gameObject);

	}
}
