using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {
//Destroys any object which hits the boundary, aka any object that flies off screen.
//This script is applide to the Boundary gameobject.



	void OnTriggerExit(Collider other)
	{
		Destroy(other.gameObject);
	}
}
