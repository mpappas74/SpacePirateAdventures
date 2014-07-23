using UnityEngine;
using System.Collections;

public class Level3Controller : Level_Controller
	//Level3Controller, or controllers for any other levels, are the scripts that vary for each level. 
	//They inherit from Level_Controller.
	//I've hardcoded a bunch of variables into Start - this overwrites the values they are given in the inspector. 
	
{	
	
	public override void Start ()
	{
		startWait = new float[2]{0, 0};
		hazardCount = new float[2]{10, 3};
		spawnValues = new Vector3[2]{new Vector3(75.0f, 0.0f, 5.5f), new Vector3(80.0f, 0.0f, 5.5f)};
		hazardNames = new string[2]{"EnemyBombShip", "EnemyBasicShip"};
		spawnWait = new float[2]{0.2f, 1.5f};
		waveWait = new float[2]{0, 4};
		numberWaves = new int[2]{0, 300};
		startStreamNow = new int[2]{0, 1};
		
		lanes = new int[2]{-1, -2};
		health = new float[2]{1, 4};
		shieldHealth = new float[2]{0, 0};
		score = new float[2]{2, 6};		
		
		waveDifficultyVariable = new float[2]{1,1};
		levelDifficultyVariable = new float[2]{1,1};
		numWavesBeforeIncrement = new int[2]{1000,1000};

		base.Start();
	}
	
	public override void Update ()
	{
		base.Update();
	}
	
}
