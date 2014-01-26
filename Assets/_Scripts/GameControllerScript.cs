using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour
{
	public GameObject tinyShip; //The tinyShip prefab.
	public GameObject crazyShip; //The crazyShip prefab.
	public GameObject shieldShip; //The shieldShip prefab.
	public GameObject bombShip; //The bombShip prefab.
	public GameObject shield; //The shield prefab.
	public GameObject placingBox; //The placingBox prefab.
	public GameObject notEnoughMoneyObject; //The notEnoughMoneyObject prefab.
	private float score; //The total score across multiple levels.
	private int currentUnlockedLevel; //The highest level that has currently been unlocked.
	private int currentLevel; //The level that is currently or most recently played.

	//Now begins the probably huge stream of variables that determine particular ship upgrades.
	private float tinyShipDamage;	//Damage done by a shot from the tinyShip.
	
	void Awake ()
	{
		DontDestroyOnLoad(transform.gameObject);
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
	public void prepareAllShips(){
		tinyShip.GetComponent<TinyShipMover>().tinyShipDamage = tinyShipDamage;
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
	public GameObject getShield(){
		return shield;
	}
	public GameObject getPlacingBox(){
		return placingBox;
	}
	public GameObject getNotEnoughMoneyObject(){
		return notEnoughMoneyObject;
	}
	
	//TinyShip setter. I currently have two versions in case we don't want to save to playerPrefs every time something changes.
	//Depending on how frequently things happen, this may or may not be the way to go.
	public void setTinyShip(float tsD){
		tinyShipDamage = tsD;
	}
	public void prefSetTinyShip(float tsD){
		tinyShipDamage = tsD;
		PlayerPrefs.SetFloat("TinyShipDamage", tinyShipDamage);
	}
	
}
