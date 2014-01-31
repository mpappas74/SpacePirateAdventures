using UnityEngine;
using System.Collections;

public class HealthTracker : MonoBehaviour
{
	//Set these two in the inspector for each ship prefab!
	public float health; //The health of the ship.
	public float scoreVal;	//How much the ship is worth in points after being destroyed.

	public bool isDead;	//Whether the ship is dead - this allows particular ship controller scripts to handle death differently.
	private float maxHealth; //Maximum health of the ship
	private float maxLength; //Maximum length of the ship's healthbar.
	
	void Start ()
	{
		//Figure out the maximum health and length.
		maxHealth = health;
		Transform healthbar = gameObject.transform.Find ("HealthBar");
		if (healthbar != null) {
			maxLength = healthbar.localScale.x;
		}
	}

	public void DecreaseHealth (float healthChange)
	{
		//Decrease health according to the damage done. If the ship is down to 0 health, destroy it,
		//add the score to the player's points, and cause the explosion to occur.
		health -= healthChange;
		if (health <= 0) {	
			isDead = true;
			LevelController lController = GameObject.Find ("LevelController").GetComponent<LevelController> ();
			lController.levelScore += scoreVal;
		}
		//If the ship has a healthbar attached to it (shields, for instance, currently don't), decrease the length of the healthbar according to damage done.
		Transform healthbar = gameObject.transform.Find ("HealthBar");
		if (healthbar != null) {
			healthbar.localScale = new Vector3 (maxLength * health / maxHealth, healthbar.localScale.y, healthbar.localScale.z);
		}
	}
}
