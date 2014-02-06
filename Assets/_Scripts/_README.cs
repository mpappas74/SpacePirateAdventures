using UnityEngine;
using System.Collections;

public class _README : MonoBehaviour
{
//This ReadMe is for explaining how to build a level using the currently implemented scripts. Individual scripts have comments
//explaining the logic withing them, but this is to show how they are meant to combine.

//The main game itself starts at the main menu. This main menu has two components - it has the main menu script
//which handles the button presses on the main menu, and the gameControllerScript, which is a Singleton and will instantiate itself when an instance of it is called.
//The GameController script DOES NOT DIE, and can be accessed at any point in the game in any scene. It holds things like
//templates for the various ships (with upgrade information), the total game score, and the currently unlocked levels.

//From this, we can enter a level. Basic elements, such as a background and boundary (to destroy things which fly offscreen) can
//be copied trivially from the first level example. More important is a discussion of a few important objects.

//The mainCamera clearly should have the CameraScript attached. The parameters of this script determine the camera's movement.

//The EmptyButtonObject should be given ButtonHandler.cs. This script just handles the APPEARANCE AND GUI RENDERING of buttons.
//ALL LOGIC RELATED TO THE BUTTONS BEING PRESSED IS IN LevelController.

//Next, let's discuss the LevelController object. This has two scripts, LevelController and LevelXController, where X is the number of the current level.
//LevelController should not change between levels. It handles the building of ships according to the buttons that have been pressed.
//LevelXController should be a different script for each level, and handles the generation of enemy ships. It should also determing win conditions.
//LevelXControllers should all inherit from Level_Controller.

//Lanes also need to be established. If there are levels without lanes, currently, MoveInLane will still behave as we probably desire, although it will have a lot of worthless code.
//If levels without lanes become common, we can disable/enable MoveInLane and MoveForward in LevelXController.
	
//There is also a miniMap. The miniMap will mostly handle itself, but it is important to make sure that the Main Camera script has access to it through the inspector.
//This will keep the miniMap always aligned properly in-game.

//Finally, we have motherships. The player and enemy motherships can have their health and properties updated independently.
//Make sure that you have GUITexts for both mothersihps, though, and that both have access to those GUITexts through the inspector.

}
