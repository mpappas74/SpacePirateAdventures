using UnityEngine;
using System.Collections;
using System;

public class GameControllerScript : Singleton<GameControllerScript>
{
	protected GameControllerScript(){}
	public GameObject basicShip; //The tinyShip prefab.
	public GameObject crazyShip; //The crazyShip prefab.
	public GameObject shieldShip; //The shieldShip prefab.
	public GameObject bombShip; //The bombShip prefab.
	public GameObject stealthShip; //The stealthShip prefab.
	public GameObject shield; //The shield prefab.
	public GameObject placingBox; //The placingBox prefab.
	public GameObject loadingBar; //The placingBox prefab.
	public GameObject notEnoughMoneyObject; //The notEnoughMoneyObject prefab.
	public GameObject testObject;
	private float score; //The total score across multiple levels.
	private int currentUnlockedLevel; //The highest level that has currently been unlocked.
	private int currentLevel; //The level that is currently or most recently played.
	private float gameVolume; //The relative volume level for the game.

	//Now begins the probably huge stream of variables that determine particular ship upgrades.
	private int basicShipUpgrade = 0;	
	private int fighterShipUpgrade = 0;	
	private int stealthShipUpgrade = 0;	
	private int motherShipUpgrade = 0;	
	
	public bool IncreasedStartingScore = false;
	public bool IncreasedScoreRate = false;
	public bool mothershipHealth = false;

	private bool canSetUpShipsNow = false;

	public bool[] hasObtainedUpgrade = new bool[12];



	void Awake ()
	{
		DontDestroyOnLoad(this);

		StartCoroutine("GetShips");
		
		//Check PlayerPrefs to see if the three main things are saved. If not, set them to default levels.
		if(PlayerPrefs.HasKey("Score")){
			score = PlayerPrefs.GetFloat("Score");
		} else{
			score = 10;
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
		if(PlayerPrefs.HasKey("BasicShipUpgrades")){
			basicShipUpgrade = PlayerPrefs.GetInt("BasicShipUpgrades");
		} else {
			basicShipUpgrade = 0;
		}
		if(PlayerPrefs.HasKey("FighterShipUpgrades")){
			fighterShipUpgrade = PlayerPrefs.GetInt("FighterShipUpgrades");
		} else {
			fighterShipUpgrade = 0;
		}
		if(PlayerPrefs.HasKey("StealthShipUpgrades")){
			stealthShipUpgrade = PlayerPrefs.GetInt("StealthShipUpgrades");
		} else {
			stealthShipUpgrade = 0;
		}
		if(PlayerPrefs.HasKey("MotherShipUpgrades")){
			motherShipUpgrade = PlayerPrefs.GetInt("MotherShipUpgrades");
		} else {
			motherShipUpgrade = 0;
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
		basicShip = (GameObject)Resources.Load("BasicShip"); //The basicShip prefab.
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
		basicShip = (GameObject)Instantiate(basicShip, new Vector3(0.0f, 0.0f, 0.0f), basicShip.transform.rotation);
		basicShip.SetActive(false);
		DontDestroyOnLoad(basicShip);
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
		basicShip.GetComponent<ShipHandler>().ActivateUpgrades(basicShipUpgrade);
		shieldShip.GetComponent<ShipHandler>().ActivateUpgrades(fighterShipUpgrade);
		stealthShip.GetComponent<ShipHandler>().ActivateUpgrades(stealthShipUpgrade);

		int UpgradeInt = motherShipUpgrade;
		if(UpgradeInt % 10 == 1){
			UpgradeInt = UpgradeInt - 1;
			IncreasedStartingScore = true;
		}
		UpgradeInt = UpgradeInt/10;
		if(UpgradeInt % 10 == 1){
			UpgradeInt = UpgradeInt - 1;
			IncreasedScoreRate = true;
		}
		UpgradeInt = UpgradeInt/10;
		if(UpgradeInt % 10 == 1){
			UpgradeInt = UpgradeInt - 1;
			mothershipHealth = true;
		}

		PlayerPrefs.SetInt("BasicShipUpgrades", basicShipUpgrade);
		PlayerPrefs.SetInt("FighterShipUpgrades", fighterShipUpgrade);
		PlayerPrefs.SetInt("StealthShipUpgrades", stealthShipUpgrade);
		PlayerPrefs.SetInt("MotherShipUpgrades", motherShipUpgrade);
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
	public GameObject getBasicShip(){
		return basicShip;
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
	public void setBasicShip(int newUpgrade){
		basicShipUpgrade += (int)Math.Pow(10, newUpgrade);
	}
	public void setFighterShip(int newUpgrade){
		fighterShipUpgrade += (int)Math.Pow(10, newUpgrade);
	}
	public void setStealthShip(int newUpgrade){
		stealthShipUpgrade += (int)Math.Pow(10, newUpgrade);
	}
	public void setMotherShip(int newUpgrade){
		motherShipUpgrade += (int)Math.Pow(10, newUpgrade);
	}
	

	
}
