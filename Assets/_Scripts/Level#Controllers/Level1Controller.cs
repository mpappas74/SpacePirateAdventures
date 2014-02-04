using UnityEngine;
using System.Collections;

public class Level1Controller : Level_Controller
	//Level1Controller, or controllers for any other levels, are the scripts that vary for each level. 
	//They inherit from Level_Controller.
	//I've hardcoded a bunch of variables into Start - this overwrites the values they are given in the inspector. 
{
	public override void Start ()
	{
		startWait = new float[1]{0};
		hazardCount = new float[1]{4};
		spawnValues = new Vector3[1]{new Vector3(80.0f, 0.0f, 5.5f)};
		hazardNames = new string[1]{"EnemyTinyShip"};
		spawnWait = new float[1]{1.5f};
		waveWait = new float[1]{3};
		numberWaves = new int[1]{1};
		
		sMIL = new bool[1]{false};
		health = new float[1]{2};
		shieldHealth = new float[1]{0};
		score = new float[1]{10};

		base.Start();
	}
	

	public override void Update ()
	{
		base.Update();
	}

}
