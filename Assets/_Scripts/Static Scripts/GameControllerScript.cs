using UnityEngine;
using System.Collections;

public static class GameControllerScript
{
	public static GameObject tinyShip = (GameObject)Resources.Load("TinyShip"); //The tinyShip prefab.
	public static GameObject crazyShip = (GameObject)Resources.Load("CrazyShip"); //The crazyShip prefab.
	public static GameObject shieldShip = (GameObject)Resources.Load("ShieldShip"); //The shieldShip prefab.
	public static GameObject bombShip = (GameObject)Resources.Load("BombShip"); //The bombShip prefab.
	public static GameObject shield = (GameObject)Resources.Load("Shield"); //The shield prefab.
	public static GameObject placingBox = (GameObject)Resources.Load("PlacementBox"); //The placingBox prefab.
	public static GameObject loadingBar = (GameObject)Resources.Load("LoadingBar"); //The placingBox prefab.
	public static GameObject notEnoughMoneyObject = (GameObject)Resources.Load("NotEnoughMoneyObject"); //The notEnoughMoneyObject prefab.
	private static float score; //The total score across multiple levels.
	private static int currentUnlockedLevel; //The highest level that has currently been unlocked.
	private static int currentLevel; //The level that is currently or most recently played.

	//Now begins the probably huge stream of variables that determine particular ship upgrades.
	private static float tinyShipDamage;	//Damage done by a shot from the tinyShip.
	
	static GameControllerScript ()
	{
		//Check PlayerPrefs to see if the three main things are saved. If not, set them to default levels.
		if(PlayerPrefs.HasKey("Score")){
			score = PlayerPrefs.GetFloat("Score");
		} else{
			score = 0;
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

		prepareAllShips();
	}
	
	//This method will set all relevant variables. It should be called at the end of Start() as well as any time we have upgraded and then entered a new level.
	public static void prepareAllShips(){
		tinyShip.GetComponent<ShipHandler>().shotDamage = tinyShipDamage;
	}

	//A series of getters and setters to modify the prefabs. 
	//Setters for various ships and other gameObjects will be the upgrade methods called.
	public static void prefSetCurrentLevel(int L)
	{
		currentLevel = L;
		PlayerPrefs.SetInt("CurrentLevel", currentLevel);
	}
	public static void prefSetCurrentUnlockedLevel(int L)
	{
		currentUnlockedLevel = L;
		PlayerPrefs.SetInt("CurrentUnlockedLevel", currentUnlockedLevel);
	}
	public static void prefSetScore(float s)
	{
		score = s;
		PlayerPrefs.SetFloat("Score", score);
	}
	public static void setScore(float s){
		score = s;
	}
	public static void setCurrentUnlockedLevel(int L){
		currentUnlockedLevel = L;
	}
	public static void setCurrentLevel(int L){
		currentLevel = L;
	}
	public static float getScore(){
		return score;
	}
	public static int getCurrentLevel(){
		return currentLevel;
	}
	public static int getCurrentUnlockedLevel(){
		return currentUnlockedLevel;
	}
	public static GameObject getTinyShip(){
		return tinyShip;
	}
	public static GameObject getCrazyShip(){
		return crazyShip;
	}
	public static GameObject getShieldShip(){
		return shieldShip;
	}
	public static GameObject getBombShip(){
		return bombShip;
	}
	public static GameObject getShield(){
		return shield;
	}
	public static GameObject getPlacingBox(){
		return placingBox;
	}
	public static GameObject getLoadingBar(){
		return loadingBar;
	}
	public static GameObject getNotEnoughMoneyObject(){
		return notEnoughMoneyObject;
	}
	
	//TinyShip setter. I currently have two versions in case we don't want to save to playerPrefs every time something changes.
	//Depending on how frequently things happen, this may or may not be the way to go.
	public static void setTinyShip(float tsD){
		tinyShipDamage = tsD;
	}
	public static void prefSetTinyShip(float tsD){
		tinyShipDamage = tsD;
		PlayerPrefs.SetFloat("TinyShipDamage", tinyShipDamage);
	}
	
}
