using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour
//This script goes on the Wall in the Prefab folder.
{

	public float lifetime; //How long the wall is allowed to last.
	private float creationTime; //When the wall was made.

	// Use this for initialization
	void Start ()
	{
		creationTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{	
		//The wall dies after its lifetime has passed.
		if (Time.time > creationTime + lifetime) {
			Destroy (gameObject);
		}
	}
}
