using UnityEngine;
using System.Collections;

public class Level4Controller : Level_Controller
	//Level3Controller, or controllers for any other levels, are the scripts that vary for each level. 
	//They inherit from Level_Controller.
	//I've hardcoded a bunch of variables into Start - this overwrites the values they are given in the inspector. 
	
{	
	
	public override void Start ()
	{
		startWait = new float[3]{0, 0, 2f};
		hazardCount = new float[3]{10, 3, 3};
		spawnValues = new Vector3[3]{new Vector3(75.0f, 0.0f, 5.5f), new Vector3(80.0f, 0.0f, 5.5f), new Vector3(80.0f, 0.0f, 5.5f)};
		hazardNames = new string[3]{"EnemyBombShip", "EnemyBasicShip", "EnemyCrazyShip"};
		spawnWait = new float[3]{0.2f, 4, 4};
		waveWait = new float[3]{0, 10, 10};
		numberWaves = new int[3]{0, 4, 4};
		startStreamNow = new int[3]{0, 1, 1};
		
		lanes = new int[3]{-1, -2, -2};
		health = new float[3]{1, 4, 4};
		shieldHealth = new float[3]{0, 0, 0};
		score = new float[3]{2, 6, 6};		
		
		waveDifficultyVariable = new float[3]{1,1, 1};
		levelDifficultyVariable = new float[3]{1,1, 1};
		numWavesBeforeIncrement = new int[3]{1000,1000, 1000};

		base.Start();
	}
	
	public override void Update ()
	{
		base.Update();
	}
	
}
