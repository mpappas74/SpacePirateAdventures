using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour {

	public float speed; //The speed at which the ships fly.
	private GameObject testObject;	//The testObject holds the button information currently. Could also use GameObject.Find.
	private ButtonHandler button;	//Access to the button script to check the booleans in it. 
	
	
	// Use this for initialization
	void Start ()
	{
		rigidbody.velocity = speed * transform.forward;	//Keep the ship moving forward.
		
		testObject = GameObject.Find ("EmptyButtonObject"); //Gain access to the ButtonHandler script to determine if the game is paused.
		button = testObject.GetComponent<ButtonHandler> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//If the game is paused, don't move. Otherwise, keep moving.
		if (button.paused) {
			rigidbody.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		} else {
			rigidbody.velocity = speed * transform.forward;
		}
	}
}
