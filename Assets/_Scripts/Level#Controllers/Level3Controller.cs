using UnityEngine;
using System.Collections;

public class Level3Controller : Level_Controller
	//Level3Controller, or controllers for any other levels, are the scripts that vary for each level. 
	//They inherit from Level_Controller.
	//I've hardcoded a bunch of variables into Start - this overwrites the values they are given in the inspector. 
	
{	
	
	public override void Start ()
	{
		startWait = new float[2]{0, 10};
		hazardCount = new float[2]{10, 4};
		spawnValues = new Vector3[2]{new Vector3(75.0f, 0.0f, 5.5f), new Vector3(80.0f, 0.0f, 5.5f)};
		hazardNames = new string[2]{"EnemyBombShip", "EnemyTinyShip"};
		spawnWait = new float[2]{0.2f, 1.5f};
		waveWait = new float[2]{0, 3};
		numberWaves = new int[2]{1, 3};
		
		sMIL = new bool[2]{false, false};
		health = new float[2]{1, 4};
		shieldHealth = new float[2]{0, 0};
		score = new float[2]{2, 6};		
		
		base.Start();
	}
	
	public override void Update ()
	{
		base.Update();
	}
	
}
