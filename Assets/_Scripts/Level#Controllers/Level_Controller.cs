using UnityEngine;
using System.Collections;

public class Level_Controller : MonoBehaviour
	//The base class from which level#controllers inherit.
{
	public GameObject testObject;	//The testObject holds the button information currently. 
	private ButtonHandler button;	//Access to the button script to check the booleans in it.
	public float[] startWait;	//How long to wait before enemy ships start coming.
	public float[] hazardCount;	//How many enemy ships to send per wave.
	public Vector3[] spawnValues;	//Where to generate the enemy ships. (See SpawnWaves() for more.)
	public string[] hazardNames;	//The enemyShip object.
	private GameObject[] hazard;
	public float[] spawnWait;		//How long to wait between ships in a wave.
	public float[] waveWait;		//How long to wait between waves.
	public int[] numberWaves;
	private int[] goneWaves;
	private LevelController lc;
	public Material enemyHealthBarMaterial;
	
	
	public virtual void Start ()
	{
		button = testObject.GetComponent<ButtonHandler> ();
		lc = gameObject.GetComponent<LevelController>();

		goneWaves = new int[numberWaves.Length];
		hazard = new GameObject[hazardNames.Length];

		for(int i = 0; i < hazardNames.Length; i++){
			hazard[i] = (GameObject)Resources.Load("EnemyShips/" + hazardNames[i]); 
			hazard[i] = (GameObject)Instantiate(hazard[i], new Vector3(0, -1000, 0), hazard[i].transform.rotation);
			hazard[i].SetActive(false);
			hazard[i] = setUpHazard(hazard[i]);
			StartCoroutine(SpawnWaves(i));
		}
	}
	
	private GameObject setUpHazard(GameObject theHazard){
		ShipHandler sh = theHazard.GetComponent<ShipHandler>();
		sh.shouldMoveInLane = false;
		sh.shipHealth = 2;
		sh.energyShieldHealth = 0;
		sh.scoreValue = 10;
		theHazard.layer = LayerMask.NameToLayer("EnemyShips");
		return theHazard;
	}
	
	private bool AreGameObjectsWithLayer(int lay){
		GameObject[] goArray = (GameObject[])FindObjectsOfType(typeof(GameObject));
		int count = 0;
		foreach(GameObject go in goArray){
			if(go.layer == lay){
				count++;
			}
		}
		if(count > 0){
			return true;
		}
		return false;
	}
	
	//The coroutine/IEnumetor stuff is basically just useful for the yield option, which allows us to stall the wave generation while letting the rest of the game handle smoothly.
	IEnumerator SpawnWaves (int j)
	{
		//First, wait until the first wave is meant to start.
		yield return new WaitForSeconds (startWait[j]);
		
		//Then, we will infinitely send out new waves.
		while (goneWaves[j]<numberWaves[j]) {
			goneWaves[j]++;
			//Send out each hazard in the wave one at a time.
			for (int i = 0; i < hazardCount[j]; i++) {
				while (button.paused) {
					yield return new WaitForFixedUpdate ();
				}
				//We spawn them at a range of z values, but always at the given x and y values. Note that Random.Range is inclusive on the lower end (-2), but not on the upper end. So it will return -2, -1, 0, 1, or 2.
				Vector3 spawnPosition = new Vector3 (spawnValues[j].x, spawnValues[j].y, (spawnValues[j].z / 2) * Random.Range (-2, 3));
				GameObject newHazard = (GameObject)Instantiate (hazard[j], spawnPosition, hazard[j].transform.rotation);
				newHazard.SetActive(true);
				//Now wait until the next hazard is meant to spawn.
				yield return new WaitForSeconds (spawnWait[j]);
			}
			//Now wait until the next wave should happen.
			yield return new WaitForSeconds (waveWait[j]);
			
		}
		while(AreGameObjectsWithLayer(LayerMask.NameToLayer("EnemyShips"))){
			yield return new WaitForSeconds(1);
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