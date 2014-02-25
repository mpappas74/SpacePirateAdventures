using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
//This script destroys certain objects as they collide with asteroids.
//This script is added to the asteroid gameobject. (Found in the Prefabs folder.)
{
	
	public GameObject explosion;  //The explosion animation.
	public GameObject playerExplosion;  //The playerExplosion animation.
	public int scoreValue;	//How much the destruction of an asteroid is worth.
	private GameController gameController;	//Access to the gameController to update the score.
	

	void Start ()
	{
		//This all is basically just looking for the GameController object.
		//This whole section could be removed, and in its place we could declare gameController as a public variable above and then set it in Unity.
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			//This line gets access to the GameController script, so we can use functions declared in it to adjust the score.
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}
	

	void OnTriggerEnter(Collider other) 
	{
		//We don't do anything special if we hit the boundary, since DestroyByBoundary will handle it.
		if (other.tag == "Boundary")
		{
			return;
		}
		//Any other collision will generate an explosion.
		Instantiate(explosion, transform.position, transform.rotation);

		//If we hit the Mothership, then MotherShipController will handle the destruction of the asteroid.
		//This is probably a bit redundant, but allows for MotherShipController to carefully track the mothership health.
		if (other.tag == "Mothership")
		{
			return;
		}

		//If we hit a Wall, only the asteroid is destroyed, since the wall lasts a certain amount of time on its own.
		if (other.tag == "Wall")
		{
			Destroy(gameObject);
			return;
		}

		//It we hit the player, kill the player and end the game.
		if (other.tag == "Player")
		{
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver ();
			Destroy(other.gameObject);
			Destroy(gameObject);
			return;
		}

		//Note that we only reach this point if the tag was none of the above, which in this case means it was a bolt.
		//We could also do another if statement to check.

		//If it is a bolt, our score increases and we destroy the bolt and asteroid.
		gameController.AddScore (scoreValue);
		Destroy(other.gameObject);
		Destroy(gameObject);
	}
}