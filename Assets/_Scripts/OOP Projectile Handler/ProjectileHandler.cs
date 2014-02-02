﻿using UnityEngine;
using System.Collections;

public class ProjectileHandler : MonoBehaviour
	
//This class controls the fired projectiles.
{	
	//************** Common Properties of All Projectiles ********************//
	public float survivalTime; //survivalTime changes how long a bolt can last after being shot before vanishing.
	public bool amPlayers; //amPlayers just checks whether it was fired by the player or enemy.
	public float speed;
	
	//************** Damage Projectile Logic ********************//
	public bool doesSingleShotDamage;
	public float damageDone; //damageDone is set by the firing ship, and controls how much damage the bolt does.

	//************** Button Logic ********************//
	private GameObject testObject;	//The testObject holds the button information currently. Could also use GameObject.Find.
	private ButtonHandler button;	//Access to the button script to check the booleans in it. 
	
	//************** Explosion Logic ********************//
	public bool isExplosion;
	public float explosionDamage;
	public bool destroysBolts;
	public bool destroysShields;

	//************** Health Logic ********************//
	public bool takesDamage;
	public bool givesDamageBack;
	public float damageFromShip;
	public float damageBack;
	public float projectileHealth;
	public bool isDead;	//Whether the ship is dead - this allows particular ship controller scripts to handle death differently.
	private float maxHealth; //Maximum health of the ship
	private float maxLength; //Maximum length of the ship's healthbar.
	private Transform healthbar;


	//***************************************** Virtual Methods ******************************************************//
	

	public virtual void Start ()
	{
		if(audio != null){
		audio.Play (); //When the bolt is shot, play the shooting audio.
		}
		if(rigidbody != null){
			rigidbody.velocity = speed * transform.forward;	//Keep the ship moving forward.
		}
		testObject = GameObject.Find ("EmptyButtonObject"); //Gain access to the ButtonHandler script to determine if the game is paused.
		button = testObject.GetComponent<ButtonHandler> ();
		if (survivalTime > 0) {
			StartCoroutine ("KillSelf");
		}
		if (projectileHealth > 0) {
			maxHealth = projectileHealth;
			healthbar = gameObject.transform.Find ("HealthBar");
			if (healthbar != null) {
				maxLength = healthbar.localScale.x;
			} else {
				maxLength = -1;
			}
		}
	}

	// Update is called once per frame
	public virtual void FixedUpdate ()
	{
		if (maxHealth > 0 && isDead) {
			Destroy (gameObject);
		}
		//If the game is paused, don't move. Otherwise, keep moving.
		if (button.paused && rigidbody != null) {
			rigidbody.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		} else if(rigidbody != null) {
			rigidbody.velocity = speed * transform.forward;
		}
	}

	public virtual void OnTriggerStay (Collider other)
	{
		if (doesSingleShotDamage) {
			//Right now, these tags exist to keep bolts from hitting each other or the mothership,
			// or trying to 'damage' the outer boundary that destroys all gameObjects when offscreen.
			if (other.tag == "EnemyShip" && amPlayers) {
				other.gameObject.GetComponent<ShipHandler> ().DecreaseHealth (damageDone);
				Destroy (gameObject);
			} else if (!amPlayers && (other.tag == "CrazyShip" || other.tag == "TinyShip" || other.tag == "BombShip" || other.tag == "ShieldShip" || other.tag == "StealthShip")) {
				other.gameObject.GetComponent<ShipHandler> ().DecreaseHealth (damageDone);	
				Destroy (gameObject);	//Destroy the bolt. Whether or not the ship is destroyed is handled in DecreaseHealth.
			} else if(!amPlayers && other.tag == "Player"){
				other.gameObject.GetComponent<MothershipScript>().health -= damageDone;
				Destroy(gameObject);
			}
		}
		if (isExplosion) {
			if (other.tag == "EnemyShip" || other.tag == "CrazyShip" || other.tag == "TinyShip" || other.tag == "BombShip" || other.tag == "ShieldShip" || other.tag == "StealthShip"){
				other.gameObject.GetComponent <ShipHandler> ().DecreaseHealth (explosionDamage);	//Inflict damage. 
			} else if (other.tag == "Bolt" && destroysBolts) {
				Destroy (other.gameObject);
			} else if (other.tag == "Shield" && destroysShields) {
				Destroy (other.gameObject);
			} else if (other.tag == "Player"){
				other.gameObject.GetComponent<MothershipScript>().health -= explosionDamage/2;
			}
		}
		
		if (takesDamage) {
			if(!givesDamageBack){
				damageBack = 0;
			}
			//Depending on the colliding object, either let it pass, take a hit from it, or destroy it and take a hit from it.
			if (other.tag == "Bolt") {
				if (!other.gameObject.GetComponent<ProjectileHandler> ().amPlayers) {
					DecreaseHealth (other.gameObject.GetComponent<ProjectileHandler> ().damageDone);
					Destroy (other.gameObject);
				}
			} else if (other.tag == "EnemyShip") {
				DecreaseHealth (damageFromShip);
				other.gameObject.GetComponent<ShipHandler>().DecreaseHealth(damageBack);
			} else {
				//Debug.Log(other.tag);
			}
		}
	}

	//***************************************** Standard Methods ******************************************************//
	
	public void DecreaseHealth (float healthChange)
	{
		//Decrease health according to the damage done. If the ship is down to 0 health, destroy it,
		//add the score to the player's points, and cause the explosion to occur.
		projectileHealth -= healthChange;
		if (projectileHealth <= 0) {	
			isDead = true;
		}
		//If the ship has a healthbar attached to it (shields, for instance, currently don't), decrease the length of the healthbar according to damage done.
		if (maxLength > 0) {
			healthbar.localScale = new Vector3 (maxLength * projectileHealth / maxHealth, healthbar.localScale.y, healthbar.localScale.z);
		}
	}
	

	//***************************************** Coroutines ******************************************************//
	
	public IEnumerator KillSelf ()
	{
		//This coroutine will make the bolts disappear after a set amount of time if they fail to hit anything.
		yield return new WaitForSeconds (survivalTime);
		Destroy (gameObject);
		
	}
	
}
