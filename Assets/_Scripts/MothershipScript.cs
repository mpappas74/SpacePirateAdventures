using UnityEngine;
using System.Collections;

public class MothershipScript : MonoBehaviour {

	public GUIText healthText;
	public float health;
	private float wasHealth;

	// Use this for initialization
	void Start () {
		wasHealth = health;
		healthText.text = "Mothership Health = " + health.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		if(wasHealth != health){
			if(health <= 0){
				health = 0;
			}
			healthText.text = "Mothership Health = " + health.ToString();
		}
	}

}
