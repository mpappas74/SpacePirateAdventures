using UnityEngine;
using System.Collections;

public class ShieldScript : MonoBehaviour
{

	public GameObject explosion; //The particular explosion animation for when a ship explodes against the shield.
	
	void Update ()
	{
		//If we run out of health, kill ourselves.
		if (GetComponent<HealthTracker> ().isDead) {
			Destroy (gameObject);
		}
	}

	// OnTriggerStay (rather than OnTriggerEnter) is used in case the shield is formed as an enemyShip or bolt is passing through.
	void OnTriggerStay (Collider other)
	{
		//Depending on the colliding object, either let it pass, take a hit from it, or destroy it and take a hit from it.
		if (other.tag == "Bolt") {
			if (!other.gameObject.GetComponent<BoltMover> ().amPlayersBolt) {
				GetComponent<HealthTracker> ().DecreaseHealth (other.gameObject.GetComponent<BoltMover> ().damageDone);
				Destroy (other.gameObject);
			}
		} else if (other.tag == "EnemyShip") {
			GetComponent<HealthTracker> ().DecreaseHealth (10);
			Instantiate (explosion, other.gameObject.transform.position, Quaternion.identity);
			LevelController lController = GameObject.Find ("LevelController").GetComponent<LevelController> ();
			lController.levelScore += other.gameObject.GetComponent<HealthTracker>().scoreVal;
			Destroy (other.gameObject);
		} else {
			//Debug.Log(other.tag);
		}
	}
}
