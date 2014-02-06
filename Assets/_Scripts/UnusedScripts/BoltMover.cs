using UnityEngine;
using System.Collections;

public class BoltMover : MonoBehaviour

//This class controls the fired bolts. It moves them whenever the game is not paused and inflicts damage on collisions.
{	
	public float survivalTime; //survivalTime changes how long a bolt can last after being shot before vanishing.
	public bool amPlayersBolt; //amPlayersBolt just checks whether it was fired by the player or enemy.
	public float damageDone; //damageDone is set by the firing ship, and controls how much damage the bolt does.

	void Start ()
	{
		audio.Play (); //When the bolt is shot, play the shooting audio.
		StartCoroutine ("KillSelf");
	}
	
	public IEnumerator KillSelf ()
	{
		//This coroutine will make the bolts disappear after a set amount of time if they fail to hit anything.
		yield return new WaitForSeconds (survivalTime);
		Destroy (gameObject);
		
	}

	void OnTriggerEnter (Collider other)
	{	
		//Right now, these tags exist to keep bolts from hitting each other or the mothership,
		// or trying to 'damage' the outer boundary that destroys all gameObjects when offscreen.
		if (other.tag == "EnemyShip" && amPlayersBolt){
			other.gameObject.GetComponent<HealthTracker>().DecreaseHealth(damageDone);
			Destroy(gameObject);
		} else if(!amPlayersBolt && (other.tag == "CrazyShip" || other.tag == "TinyShip" || other.tag == "BombShip")) {
				other.gameObject.GetComponent<ShipHandler>().DecreaseHealth (damageDone);	
				Destroy (gameObject);	//Destroy the bolt. Whether or not the ship is destroyed is handled in DecreaseHealth.
		} else {
			//Debug.Log(other.tag);
		}
		
	}
}
