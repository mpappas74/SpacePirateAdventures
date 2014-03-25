using UnityEngine;
using System.Collections;

public class Level_Controller : MonoBehaviour
	//The base class from which level#controllers inherit.
{
	public float[] startWait;	//How long to wait before enemy ships start coming.
	public float[] hazardCount;	//How many enemy ships to send per wave.
	public Vector3[] spawnValues;	//Where to generate the enemy ships. (See SpawnWaves() for more.)
	public string[] hazardNames;	//The enemyShip types.
	private GameObject[] hazard;	//The enemyShips
	public float[] spawnWait;		//How long to wait between ships in a wave.
	public float[] waveWait;		//How long to wait between waves.
	public int[] numberWaves;	//How many attack waves of each type?
	private int[] goneWaves;	//How many attack waves have happened so far?
	private LevelController lc;
	private float[] startPos;
	public int[] lanes;
	public float[] health;
	public float[] shieldHealth;
	public float[] score;
	
	public virtual void Start ()
	{
		lc = gameObject.GetComponent<LevelController> ();
		startPos = lc.endPositions;
		//Someone sometime should really write a check that all of these arrays are the same length.
		//But I haven't yet.
		goneWaves = new int[numberWaves.Length];
		hazard = new GameObject[hazardNames.Length];

		//Run coroutines for each of the different described wave fronts.
		for (int i = 0; i < hazardNames.Length; i++) {
			loadHazard (i);
		}
	}
	
	private void loadHazard (int i)
	{
		hazard [i] = (GameObject)Resources.Load ("EnemyShips/" + hazardNames [i]); 
		hazard [i] = (GameObject)Instantiate (hazard [i], new Vector3 (0, 0, 0), hazard [i].transform.rotation);
		hazard [i].SetActive (false);
		ShipHandler sh = hazard [i].GetComponent<ShipHandler> ();
		if (sh != null) {
			sh.laneID = lanes [i];
			sh.shipHealth = health [i];
			sh.energyShieldHealth = shieldHealth [i];
			sh.scoreValue = score [i];
		}	
			hazard [i].layer = LayerMask.NameToLayer ("EnemyShips");
			StartCoroutine (SpawnWaves (i));
	}
	
	//Determine if there are any GameObjects left in a certain physics layer (we will use this to tell if any enemies are left.)
	private bool AreGameObjectsWithLayer (int lay)
	{
		GameObject[] goArray = (GameObject[])FindObjectsOfType (typeof(GameObject));
		int count = 0;
		foreach (GameObject go in goArray) {
			if (go.layer == lay) {
				count++;
			}
		}
		if (count > 0) {
			return true;
		}
		return false;
	}
	
	//The coroutine/IEnumetor stuff is basically just useful for the yield option, which allows us to stall the wave generation while letting the rest of the game handle smoothly.
	IEnumerator SpawnWaves (int j)
	{
		//First, wait until the first wave is meant to start.
		yield return new WaitForSeconds (startWait [j]);
		
		//Then, we will infinitely send out new waves.
		while (goneWaves[j]<numberWaves[j]) {
			goneWaves [j]++;
			//Send out each hazard in the wave one at a time.
			for (int i = 0; i < hazardCount[j]; i++) {
				//We spawn them at a range of z values, but always at the given x and y values. Note that Random.Range is inclusive on the lower end (-2), but not on the upper end. So it will return -2, -1, 0, 1, or 2.
				Vector3 spawnPosition = new Vector3 (spawnValues [j].x, spawnValues [j].y, (spawnValues [j].z / 2) * Random.Range (-2, 3));
				GameObject newHazard;
				if (lanes [j] != -1) {
					if (lanes [j] == -2) {
						int laneIndex = Random.Range (0, startPos.Length);
						spawnPosition.z = startPos [laneIndex];
						newHazard = (GameObject)Instantiate (hazard [j], spawnPosition, hazard [j].transform.rotation);
						ShipHandler sh = newHazard.GetComponent<ShipHandler> ();
						if (sh != null) {
							sh.laneID = laneIndex;
							sh.isThisPlayers = false;
						}
					} else {
						spawnPosition.z = startPos [lanes [j]];
						newHazard = (GameObject)Instantiate (hazard [j], spawnPosition, hazard [j].transform.rotation);
					}
				} else {
					newHazard = (GameObject)Instantiate (hazard [j], spawnPosition, hazard [j].transform.rotation);
				}
				newHazard.SetActive (true);
				//Now wait until the next hazard is meant to spawn.
				yield return new WaitForSeconds (spawnWait [j]);
			}
			//Now wait until the next wave should happen.
			yield return new WaitForSeconds (waveWait [j]);
			
		}
		//Once the waves are over, start checking to see if the enemy has run out of ships.
		while (AreGameObjectsWithLayer(LayerMask.NameToLayer("EnemyShips"))) {
			yield return new WaitForSeconds (1);
		}
		lc.playerVictory = true;
		
	}
	
	public virtual void Update ()
	{
		if (lc.gameOver) {
			StopAllCoroutines ();
		} 
	}
	
}