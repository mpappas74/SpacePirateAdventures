using UnityEngine;
using System.Collections;

public class Level2Controller : Level_Controller
	//Level2Controller, or controllers for any other levels, are the scripts that vary for each level. 
	//They inherit from Level_Controller.
	//I've hardcoded a bunch of variables into Start - this overwrites the values they are given in the inspector. 

{	

	public override void Start ()
	{
		startWait = new float[1]{0};
		hazardCount = new float[1]{3};
		spawnValues = new Vector3[1]{new Vector3(80.0f, 0.0f, 5.5f)};
		hazardNames = new string[1]{"EnemyBasicShip"};
		spawnWait = new float[1]{1.5f};
		waveWait = new float[1]{5};
		numberWaves = new int[1]{300};
		startStreamNow = new int[1]{0}

		lanes = new int[1]{-2};
		health = new float[1]{2};
		shieldHealth = new float[1]{0};
		score = new float[1]{5};	

		waveDifficultyVariable = new float[1]{1};
		levelDifficultyVariable = new float[1]{1};
		numWavesBeforeIncrement = new int[1]{1000};	

		base.Start();
	}
	
	public override void Update ()
	{
		base.Update();
	}
	
}
