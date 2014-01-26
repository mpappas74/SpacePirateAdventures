using UnityEngine;
using System.Collections;

public class ShieldShipMover : MonoBehaviour
	//Once they have ben built, this script moves ships and has them fire.
{
	public GameObject shield; //Access to the shield to instantiate them.
	public Transform shotSpawn; //Where to instantiate shield relative to the ship.

	private GameObject testObject;	//The testObject holds the button information currently.
	private ButtonHandler button;	//Access to the button script to check the booleans in it. 
	
	public GameObject explosion; //Access to the explosion to instantiate it when the ship dies.
	
	private InputHandler input;
	private Vector2 currentClickPos;

	void Start ()
	{
		testObject = GameObject.Find ("EmptyButtonObject");
		button = testObject.GetComponent<ButtonHandler> ();
		//As we've seen before, getting access to ButtonHandler's booleans to know if we are paused.
		//Similar logic with the game controller script.
		input = GameObject.Find ("GameController").GetComponent<InputHandler> ();
		
	}
	
	void Update ()
	{
		if (GetComponent<HealthTracker> ().isDead) {
			Destroy (gameObject);
			Instantiate (explosion, transform.position, transform.rotation);
			audio.Play ();
		}
		//If we are not paused, just keep shooting separated by the correct amount of time.
		if (!button.paused) {
			if (input.Began ()) {
				currentClickPos = input.startPos ();
			}
			if (input.Ended ()) {
				if ((input.endPos () - currentClickPos).sqrMagnitude < 2) {
					Vector3 pos = new Vector3 (currentClickPos.x, currentClickPos.y, 10.0f);
					pos = Camera.main.ScreenToWorldPoint (pos);
					if ((pos - transform.position).sqrMagnitude < 2) {
						Instantiate (shield, shotSpawn.position, shotSpawn.rotation);
						Destroy (gameObject);
					}
				}
			}
		} 
	}
	
}
