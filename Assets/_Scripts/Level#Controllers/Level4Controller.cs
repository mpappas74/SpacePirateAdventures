using UnityEngine;
using System.Collections;

public class Level4Controller : Level_Controller
	//Level3Controller, or controllers for any other levels, are the scripts that vary for each level. 
	//They inherit from Level_Controller.
	//I've hardcoded a bunch of variables into Start - this overwrites the values they are given in the inspector. 
	
{	
	
	public override void Start ()
	{
		//Terminology Reminder:: An individual hazard is one of the enemy ships.
		//A wave is a collection of hazards that all are spawned close to each other in time.
		//A stream is composed of multiple waves.

		//Also remember that the notation new X[Y]{a, b, c, ...} means an array of length Y, filled with elements of type X.
		//a,b,c,etc are those elements, and they correspond to the various different streams. So if a is "EnemyBombShip", that means 
		//that the values in the first slot in every other array correspond to the bomb ship stream.

		startStreamNow = new int[3]{0, 1, 1}; //How many streams need to have FINISHED before this stream starts.
																					//So the first stream here starts after 0 streams have finished (immediately.)
																					//The next two streams start at the same time, meaning they will be occuring alongside each other.

		startWait = new float[3]{0, 0, 2f}; //How many seconds to wait until starting the stream
																				//AFTER startStreamNow has triggered.
		
		hazardCount = new float[3]{10, 3, 3};//How many hazards per wave?

		spawnValues = new Vector3[3]{new Vector3(75.0f, 0.0f, 5.5f), new Vector3(80.0f, 0.0f, 5.5f), new Vector3(80.0f, 0.0f, 5.5f)};
		//Each Vector3 defines where the hazards spawn. The x and y values are exact, but the z values are more complicated.
		//So, the z values basically define a range. So if the z value is 5.5, that actually means that the hazards will spawn
		//randomly between -5.5 and 5.5. (NOTE THAT THIS IS IRRELEVANT UNLESS YOU CHOOSE -1 IN THE CORRESPONDING lanes ELEMENT!)		

		hazardNames = new string[3]{"EnemyBombShip", "EnemyBasicShip", "EnemyCrazyShip"};
		//Which type of ship the stream will be made from. Each stream can only consist of one hazard type.
		//If you want two hazard types to be appearing at once, they need to be two concurrent streams.

		spawnWait = new float[3]{0.2f, 4, 4}; //During a single wave, how many seconds to pause after making one hazard before making the next.

		waveWait = new float[3]{0, 10, 10}; //How long to wait between two waves of a stream.

		numberWaves = new int[3]{0, 4, 4}; //How many ADDITIONAL waves after the first one there are in a stream.
		//(This is because of the do-while loop, if you care.) In other words, the 0 means that there will be 1 wave,
		//and the 4 means that there will be 5 waves in the stream.
		
		lanes = new int[3]{-1, -2, -2};
		//If the element for a particular stream is 0, 1, 2, 3, etc, then that means that EVERY hazard in the entire stream
		//will be in that single lane.
		//If the element is -1, then that means that the hazards will not be in lanes, but will instead be randomly placed at different heights. (See SpawnValues above.)
		//If the element is -2, then that means that the hazards will be randomly placed in ANY of the available lanes.

		health = new float[3]{1, 4, 4};
		//How much health the hazards in a particular stream should have.

		shieldHealth = new float[3]{0, 0, 0};
		//Irrelevant, we never implemented shieldHealth.
	
		score = new float[3]{2, 6, 6};	
		//How many points the player should get for destroying a single hazard.	
		
		numWavesBeforeIncrement = new int[3]{1000,1000, 1000};
		//How many waves should pass before the stream increments in difficulty (see below.) So here, every 1000 waves, the stream wil get harder.
		//If you make this number negative (i.e. -3), then, rather than every 2 levels, increments will happen after level
		//3, then after SIX more levels (so at level 9), then after TWELVE more levels (at level 21), etc. In other words,
		//how many levels we wait doubles each time. You can use this if you want the waves to start easy, quickly get harder,
		//but then not get too stupidly hard too fast.

		waveDifficultyVariable = new float[3]{1,1, 1};
		//This number multiplies the spawnWait variable every time a difficulty increment happens.
		//So if you want the hazards to appear FASTER, these numbers should be LESS than 1.

		levelDifficultyVariable = new float[3]{1,1, 1};
		//Similar to waveDifficultyVariable, but it corresponds to waveWait instead.



		base.Start();
	}
	
	public override void Update ()
	{
		base.Update();
	}
	
}
