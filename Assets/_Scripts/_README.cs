using UnityEngine;
using System.Collections;

public class _README : MonoBehaviour
{
//This ReadMe is for explaining how to build a level using the currently implemented scripts. Individual scripts have comments
//explaining the logic withing them, but this is to show how they are meant to combine.

//The main game itself starts at the main menu. This main menu has two components - it has the main menu script
//which handles the button presses on the main menu, and the GameController object with the gameControllerScript.
//The GameController object DOES NOT DIE, and can be accessed at any point in the game in any scene. It holds things like
//templates for the various ships (with upgrade information), the total game score, and the currently unlocked levels.

//From this, we can enter a level. Basic elements, such as a background and boundary (to destroy things which fly offscreen) can
//be copied trivially from the first level example. More important is a discussion of a few important objects.

//The mainCamera clearly should have the CameraScript attached. The parameters of this script determine the camera's movement.

//The EmptyButtonObject should be given ButtonHandler.cs. This script just handles the APPEARANCE AND GUI RENDERING of buttons.
//ALL LOGIC RELATED TO THE BUTTONS BEING PRESSED IS IN LevelController.

//Next, let's discuss the LevelController object. This has two scripts, LevelController and LevelXController, where X is the number of the current level.
//LevelController should not change between levels. It handles the building of ships according to the buttons that have been pressed.
//LevelXController should be a different script for each level, and handles the generation of enemy ships. It should also determing win conditions.

//Lanes also need to be established. If there are levels without lanes, currently, MoveInLane will still behave as we probably desire, although it will have a lot of worthless code.
//If levels without lanes become common, we can disable/enable MoveInLane and MoveForward in LevelXController.
	
}
