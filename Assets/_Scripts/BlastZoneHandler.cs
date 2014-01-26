using UnityEngine;
using System.Collections;

public class BlastZoneHandler : MonoBehaviour
//The blastZone kills anything hit by a bomb.
{

	void Start ()
	{
		//We only want the blast zone around for a moment or two, so get rid of it quickly.
		StartCoroutine ("KillSelf");
	}

	public IEnumerator KillSelf ()
	{
		yield return new WaitForSeconds (1);
		Destroy (gameObject);
	}	

	// Anything that gets caught in the blast takes 10 damage, and bolts just get straight destroyed.
	//Note that bombs are indiscriminant between ally and foe.
	void OnTriggerStay (Collider other)
	{
		if (other.tag == "EnemyShip" || other.tag == "CrazyShip" || other.tag == "TinyShip" || other.tag == "BombShip" || other.tag == "ShieldShip") {
			HealthTracker HT = other.gameObject.GetComponent <HealthTracker> (); //Get access to the HealthTracker methods and variables.
			HT.DecreaseHealth (10);	//Inflict 10 damage. 
		} else if (other.tag == "Bolt") {
			Destroy (other.gameObject);
		}
	}
}
