using UnityEngine;
using System.Collections;

public class ProjectileHandler : MonoBehaviour
	
//This class controls the fired projectiles.
{	
	//************** Common Properties of All Projectiles ********************//
	public float survivalTime; //survivalTime changes how long a bolt can last after being shot before vanishing.
	public bool amPlayers; //amPlayers just checks whether it was fired by the player or enemy.
	public float speed;	//If it is going to move, this is the relative speed boost of the projectile over the firing ship.
	
	//************** Damage Projectile Logic ********************//
	public bool doesSingleShotDamage; //Does this projectile impact other projectiles and ships?
	public float damageDone; //damageDone is set by the firing ship, and controls how much damage the bolt does.

	//************** Explosion Logic ********************//
	public bool isExplosion; //Is this projectile an explosion?
	public float explosionDamage; //How much damage does the explosion do?
	public bool destroysBolts; //Does it destroy bolts?
	public bool destroysShields; //Does it destroy shields?

	//************** Health Logic ********************//
	public bool takesDamage; //Does this projectile take damage?
	public bool givesDamageBack; //Does it give damage on collisions?
	public float damageFromShip; //How much damage does it take from a ship colliding with it?
	public float damageBack; //How much does it deal back?
	public float projectileHealth; //What is its health.
	public bool isDead;	//Whether the ship is dead - this allows particular ship controller scripts to handle death differently.
	private float maxHealth; //Maximum health of the ship
	private float maxLength; //Maximum length of the ship's healthbar.
	private Transform healthbar;

	//************** FadeAway Logic ********************//
	public bool fadesAway = false;
	public float minAlpha = 0.25f;

	//***************************************** Virtual Methods ******************************************************//
	

	public virtual void Start ()
	{
		amPlayers = (LayerMask.NameToLayer ("PlayerAttacks") == gameObject.layer);
		if (audio != null) {
			audio.Play (); //When the bolt is shot, play the shooting audio.
		}
		if (rigidbody != null) {
			rigidbody.velocity = 1*speed * transform.forward;	//Keep the ship moving forward.
		}
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
		if(fadesAway){
			StartCoroutine("FadeAway");
		}
	}

	// Update is called once per frame
	public virtual void FixedUpdate ()
	{
		if (maxHealth > 0 && isDead) {
			Destroy (gameObject);
		}
		
		if (rigidbody != null) {
			rigidbody.velocity = 1*speed * transform.forward;
		}
	}

	public virtual void OnTriggerStay (Collider other)
	{
		//Based on the physics layers, bolts can only hit projectiles and ships belonging to the other side.
		if (doesSingleShotDamage) {
			if (other.gameObject.layer == LayerMask.NameToLayer ("EnemyShips") || other.gameObject.layer == LayerMask.NameToLayer("PlayerShips")) {
					if(other.tag == "Enemy" || other.tag == "Player"){
						Destroy(gameObject);
					} else {
						other.gameObject.GetComponent<ShipHandler> ().DecreaseHealth (damageDone);
						Destroy (gameObject);
					}
			} else if (other.gameObject.tag == "Bolt") {
				Destroy (other.gameObject);
				Destroy (gameObject);
			}
		}
		if (isExplosion) {
			if (other.gameObject.layer == LayerMask.NameToLayer ("EnemyShips") || other.gameObject.layer == LayerMask.NameToLayer ("PlayerShips")) {
				other.gameObject.GetComponent <ShipHandler> ().DecreaseHealth (explosionDamage);	//Inflict damage. 
			} else if (other.tag == "Bolt" && destroysBolts) {
				Destroy (other.gameObject);
			} else if (other.tag == "Shield" && destroysShields) {
				Destroy (other.gameObject);
			} else if (other.tag == "Player" && other.tag == "Enemy") {
				other.gameObject.GetComponent<ShipHandler> ().DecreaseHealth(explosionDamage / 2);
			}
		}
		
		if (takesDamage) {
			if (!givesDamageBack) {
				damageBack = 0;
			}
			//Depending on the colliding object, either let it pass, take a hit from it, or destroy it and take a hit from it.
			if (other.tag == "Bolt") {
				if (!other.gameObject.GetComponent<ProjectileHandler> ().amPlayers) {
					DecreaseHealth (other.gameObject.GetComponent<ProjectileHandler> ().damageDone);
					Destroy (other.gameObject);
				}
			} else if (other.gameObject.layer == LayerMask.NameToLayer ("EnemyShips") || other.gameObject.layer == LayerMask.NameToLayer ("PlayerShips")) {
				DecreaseHealth (damageFromShip);
				other.gameObject.GetComponent<ShipHandler> ().DecreaseHealth (damageBack);
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
	public IEnumerator FadeAway ()
	{
		GameObject go = null;
		foreach(Transform child in transform){
			if(child.name == "VFX"){
				go = child.gameObject;
			}
		}
		int count = 0;
		while(true){
			count++;
			yield return new WaitForSeconds (0.2f);
			Color t = go.renderer.material.GetColor("_TintColor");
			go.renderer.material.SetColor ("_TintColor", new Color(t.r, t.b, t.g, Mathf.Max(1-count*(0.2f/survivalTime), minAlpha)));
		}
	}
	
}
