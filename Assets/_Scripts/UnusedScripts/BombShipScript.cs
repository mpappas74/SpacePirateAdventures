﻿using UnityEngine;
using System.Collections;

public class BombShipScript : MonoBehaviour
{
	private InputHandler input;
	public GameObject explosion; //Explosion animation.
	public GameObject blastZone; //The physical blast zone in which things take damage from the bomb.

	void Start ()
	{
		input = GameObject.Find("LevelController").GetComponent<InputHandler>();
	}

	void Update ()
	{
		//If we have input the explosion command, or else the bomb has been shot, make it explode.
		if(input.isTrigger()){
			Explode();
		} else if(GetComponent<HealthTracker>().isDead){
			Explode();
		}
	}

	//If the bomb explodes, remove it, and replace it with an explosion and blastzone.
	public void Explode(){
		Instantiate (explosion, transform.position, transform.rotation);
		Instantiate (blastZone, transform.position, transform.rotation);
		audio.Play();
		Destroy(gameObject);
	}

}
