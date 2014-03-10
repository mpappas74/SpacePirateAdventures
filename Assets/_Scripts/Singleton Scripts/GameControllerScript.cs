using UnityEngine;
using System.Collections;

public class GameControllerScript : Singleton<GameControllerScript>
{
	protected GameControllerScript(){}
	public GameObject tinyShip; //The tinyShip prefab.
	public GameObject crazyShip; //The crazyShip prefab.
	public GameObject shieldShip; //The shieldShip prefab.
	public GameObject bombShip; //The bombShip prefab.
	public GameObject stealthShip; //The stealthShip prefab.
	public GameObject shield; //The shield prefab.
	public GameObject placingBox; //The placingBox prefab.
	public GameObject loadingBar; //The placingBox prefab.
	public GameObject notEnoughMoneyObject; //The notEnoughMoneyObject prefab.
	private float score; //The total score across multiple levels.
	private int currentUnlockedLevel; //The highest level that has currently been unlocked.
	private int currentLevel; //The level that is currently or most recently played.
	private float gameVolume; //The relative volume level for the game.

	//Now begins the probably huge stream of variables that determine particular ship upgrades.
	private float tinyShipDamage;	//Damage done by a shot from the tinyShip.
	

	private bool canSetUpShipsNow = false;



	void Awake ()
	{
		DontDestroyOnLoad(this);

		StartCoroutine("GetShips");
		
		//Check PlayerPrefs to see if the three main things are saved. If not, set them to default levels.
		if(PlayerPrefs.HasKey("Score")){
			score = PlayerPrefs.GetFloat("Score");
		} else{
			score = 0;
		}
		if(PlayerPrefs.HasKey("Volume")){
			gameVolume = PlayerPrefs.GetFloat("Volume");
		} else{
			gameVolume = 0.5f;
		}
		if(PlayerPrefs.HasKey("CurrentUnlockedLevel")){
			currentUnlockedLevel = PlayerPrefs.GetInt("CurrentUnlockedLevel");
		} else{
			currentUnlockedLevel = 1;
		}
		if(PlayerPrefs.HasKey("CurrentLevel")){
			currentLevel = PlayerPrefs.GetInt("CurrentLevel");
		} else {
			currentLevel = 0;
		}

		//tinyShipDamage
		if(PlayerPrefs.HasKey("TinyShipDamage")){
			tinyShipDamage = PlayerPrefs.GetFloat("TinyShipDamage");
		} else {
			tinyShipDamage = 1;
		}

		canSetUpShipsNow = true;
		LoadSettings();
	}

	private void LoadSettings(){
		AudioListener.volume = gameVolume;
	}

	public void setVolume(float V){
		gameVolume = V;
		AudioListener.volume = gameVolume;
	}

	IEnumerator GetShips(){
		yield return new WaitForSeconds(0.0f);
		tinyShip = (GameObject)Resources.Load("TinyShip"); //The tinyShip prefab.
		crazyShip = (GameObject)Resources.Load("CrazyShip"); //The crazyShip prefab.
		shieldShip = (GameObject)Resources.Load("ShieldShip"); //The shieldShip prefab.
		bombShip = (GameObject)Resources.Load("BombShip"); //The bombShip prefab.
		stealthShip = (GameObject)Resources.Load("StealthShip"); //The stealthShip prefab.
		shield = (GameObject)Resources.Load("Shield"); //The shield prefab.
		placingBox = (GameObject)Resources.Load("PlacementBox"); //The placingBox prefab.
		loadingBar = (GameObject)Resources.Load("LoadingBar"); //The placingBox prefab.
		notEnoughMoneyObject = (GameObject)Resources.Load("NotEnoughMoneyObject"); //The notEnoughMoneyObject prefab.
		
		//The below code is to keep clonable instances of the ships without changing the actual prefabs. When we get to 
		//the point where we don't mind prefabs being changed (ie the game is actually deployed) we can remove this bit.
		tinyShip = (GameObject)Instantiate(tinyShip, new Vector3(0.0f, 0.0f, 0.0f), tinyShip.transform.rotation);
		tinyShip.SetActive(false);
		DontDestroyOnLoad(tinyShip);
		crazyShip = (GameObject)Instantiate(crazyShip, new Vector3(0.0f, 0.0f, 0.0f), crazyShip.transform.rotation);
		crazyShip.SetActive(false);
		DontDestroyOnLoad(crazyShip);
		shieldShip = (GameObject)Instantiate(shieldShip, new Vector3(0.0f, 0.0f, 0.0f), shieldShip.transform.rotation);
		shieldShip.SetActive(false);
		DontDestroyOnLoad(shieldShip);
		bombShip = (GameObject)Instantiate(bombShip, new Vector3(0.0f, 0.0f, 0.0f), bombShip.transform.rotation);
		bombShip.SetActive(false);
		DontDestroyOnLoad(bombShip);
		stealthShip = (GameObject)Instantiate(stealthShip, new Vector3(0.0f, 0.0f, 0.0f), stealthShip.transform.rotation);
		stealthShip.SetActive(false);
		DontDestroyOnLoad(stealthShip);
		shield = (GameObject)Instantiate(shield, new Vector3(0.0f, 0.0f, 0.0f), shield.transform.rotation);
		shield.SetActive(false);
		DontDestroyOnLoad(shield);

		while(!canSetUpShipsNow){
			yield return new WaitForSeconds(0.1f);
		}
		prepareAllShips();

	}
	
	//This method will set all relevant variables. It should be called at the end of Start() as well as any time we have upgraded and then entered a new level.
	public void prepareAllShips(){
		tinyShip.GetComponent<ShipHandler>().shotDamage = tinyShipDamage;
	}

	//A series of getters and setters to modify the prefabs. 
	//Setters for various ships and other gameObjects will be the upgrade methods called.
	public void prefSetCurrentLevel(int L)
	{
		currentLevel = L;
		PlayerPrefs.SetInt("CurrentLevel", currentLevel);
	}
	public void prefSetCurrentUnlockedLevel(int L)
	{
		currentUnlockedLevel = L;
		PlayerPrefs.SetInt("CurrentUnlockedLevel", currentUnlockedLevel);
	}
	public void prefSetScore(float s)
	{
		score = s;
		PlayerPrefs.SetFloat("Score", score);
	}
	public void setScore(float s){
		score = s;
	}
	public void setCurrentUnlockedLevel(int L){
		currentUnlockedLevel = L;
	}
	public void setCurrentLevel(int L){
		currentLevel = L;
	}
	public float getScore(){
		return score;
	}
	public int getCurrentLevel(){
		return currentLevel;
	}
	public int getCurrentUnlockedLevel(){
		return currentUnlockedLevel;
	}
	public GameObject getTinyShip(){
		return tinyShip;
	}
	public GameObject getCrazyShip(){
		return crazyShip;
	}
	public GameObject getShieldShip(){
		return shieldShip;
	}
	public GameObject getBombShip(){
		return bombShip;
	}
	public GameObject getStealthShip(){
		return stealthShip;
	}
	public GameObject getShield(){
		return shield;
	}
	public GameObject getPlacingBox(){
		return placingBox;
	}
	public GameObject getLoadingBar(){
		return loadingBar;
	}
	public GameObject getNotEnoughMoneyObject(){
		return notEnoughMoneyObject;
	}
	
	//TinyShip setter. I currently have two versions in case we don't want to save to playerPrefs every time something changes.
	//Depending on how frequently things happen, this may or may not be the way to go.
	public void setTinyShip(float shipDamageIncrease){
		tinyShipDamage += shipDamageIncrease;
	}
	public void prefSetTinyShip(float tsD){
		tinyShipDamage = tsD;
		PlayerPrefs.SetFloat("TinyShipDamage", tinyShipDamage);
	}

	
}
