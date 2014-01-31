using UnityEngine;
using System.Collections;

public class Level1Controller : MonoBehaviour
//Level1Controller, or controllers for any other levels, are the scripts that vary for each level. They handle things like enemy wave generation.
{
	public GameObject testObject;	//The testObject holds the button information currently. 
	private ButtonHandler button;	//Access to the button script to check the booleans in it.
	public float startWait;	//How long to wait before enemy ships start coming.
	public float hazardCount;	//How many enemy ships to send per wave.
	public Vector3 spawnValues;	//Where to generate the enemy ships. (See SpawnWaves() for more.)
	private GameObject hazard;	//The enemyShip object.
	public float spawnWait;		//How long to wait between ships in a wave.
	public float waveWait;		//How long to wait between waves.
	private int numWaves = 0;
	private LevelController lc;
	public Material enemyHealthBarMaterial;
	
	
	void Start ()
	{
		button = testObject.GetComponent<ButtonHandler> ();
		lc = gameObject.GetComponent<LevelController>();
		StartCoroutine ("SpawnWaves");
		hazard = (GameObject)Resources.Load("EnemyShips/EnemyTinyShip"); //The tinyShip prefab.
		hazard = (GameObject)Instantiate(hazard, new Vector3(0, -1000, 0), hazard.transform.rotation);
		hazard.SetActive(false);
		hazard = setUpHazard(hazard);
	}

	private GameObject setUpHazard(GameObject theHazard){
		ShipHandler sh = theHazard.GetComponent<ShipHandler>();
		sh.shouldMoveInLane = false;
		sh.shipHealth = 2;
		sh.energyShieldHealth = 0;
		sh.scoreValue = 10;
		theHazard.tag = "EnemyShip";
		return theHazard;
	}
	
	//The coroutine/IEnumetor stuff is basically just useful for the yield option, which allows us to stall the wave generation while letting the rest of the game handle smoothly.
	IEnumerator SpawnWaves ()
	{
		//First, wait until the first wave is meant to start.
		yield return new WaitForSeconds (startWait);
		
		//Then, we will infinitely send out new waves.
		while (numWaves<1) {
			numWaves++;
			//Send out each hazard in the wave one at a time.
			for (int i = 0; i < hazardCount; i++) {
				while (button.paused) {
					yield return new WaitForFixedUpdate ();
				}
				//We spawn them at a range of z values, but always at the given x and y values. Note that Random.Range is inclusive on the lower end (-2), but not on the upper end. So it will return -2, -1, 0, 1, or 2.
				Vector3 spawnPosition = new Vector3 (spawnValues.x, spawnValues.y, (spawnValues.z / 2) * Random.Range (-2, 3));
				GameObject newHazard = (GameObject)Instantiate (hazard, spawnPosition, hazard.transform.rotation);
				newHazard.SetActive(true);
				//Now wait until the next hazard is meant to spawn.
				yield return new WaitForSeconds (spawnWait);
			}
			//Now wait until the next wave should happen.
			yield return new WaitForSeconds (waveWait);
			
		}
		while(GameObject.FindWithTag("EnemyShip") != null){
			yield return new WaitForSeconds(1);
		}
		lc.playerVictory = true;
		
	}
	
	
	void Update ()
	{
		if (lc.gameOver) {
			StopCoroutine ("SpawnWaves");
		} 
	}

}
