using UnityEngine;
using System.Collections;

public class DestroyIfOffscreen : MonoBehaviour {
	//Destroys any object which hits the boundary, aka any object that flies off screen.
	//This is places on a boundary around the entire game field, to destroy objects that are no longer part of the game.
	//Really just an efficiency thing, don't want thousands of bolts running around far offscreen.
	
	void OnTriggerExit(Collider other)
	{
		Destroy(other.gameObject);
	}
}
