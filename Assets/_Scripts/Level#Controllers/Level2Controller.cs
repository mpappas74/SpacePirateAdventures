using UnityEngine;
using System.Collections;

public class Level2Controller : Level_Controller
	//Level1Controller, or controllers for any other levels, are the scripts that vary for each level. They handle things like enemy wave generation.
{	

	public override void Start ()
	{
		startWait = new float[1]{0};
		hazardCount = new float[1]{4};
		spawnValues = new Vector3[1]{new Vector3(80.0f, 0.0f, 5.5f)};
		hazardNames = new string[1]{"EnemyTinyShip"};
		spawnWait = new float[1]{1.5f};
		waveWait = new float[1]{3};
		numberWaves = new int[1]{3};

		base.Start();
	}
	
	public override void Update ()
	{
		base.Update();
	}
	
}
