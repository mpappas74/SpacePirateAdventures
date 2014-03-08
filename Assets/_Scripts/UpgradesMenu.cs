using UnityEngine;
using System.Collections;

public class UpgradesMenu : MonoBehaviour
	//This script creates the main menu.
{
	
	//Main determines whether we are at the basic main menu (Play vs Settings vs High Scores).
	//Similarly, settingsMenu, levelMenu, and soundTest tell us we are in other deeper menus.
	//backButton determines whether there currently needs to be a button allowing return to the main menu.
	public bool main;
	public bool backButton;

	public GUISkin customSkin;
	
	//The guiText object will display information such as "There are no settings yet."
	private string text = "";
	public GUIStyle theGuiTextStyle;
	//The titleStyle determines the format of the title.
	public GUIStyle titleStyle;
	
	//This is for the upgrades grid, which will automatically nicely format our upgrade selection screen.
	private int mainShopInt = -1;
	private string[] mainUpgradeTypeStrings = {"Basic Ship", "Fighter Ship", "Stealth Ship", "Mothership"};
	
	private bool basicShipMenu;
	private bool fighterShipMenu;
	private bool stealthShipMenu;
	private bool mothershipMenu;

	private int secondaryUpgradeInt = -1;
	private string[] secondaryUpgradeStrings;
	private string[] basicShipMenuStrings = {"Health Increase", "Shield Increase", "Make Better"};

	void Start ()
	{
	}
	
	void OnGUI ()
	{
		GUI.skin = customSkin;
		//Generate the buttons in locations based on screen size to keep a consistent positioning.
		//Start by declaring the title just as a text box.
		//We also declare a currently empty text box which we will use if we need to put text onscreen anywhere in the menu.
		GUI.Box (new Rect (.5f * Screen.width - 200, .1f * Screen.height, 400, .15f * Screen.height), "Upgrades", titleStyle);
		GUI.Box (new Rect (.5f * Screen.width - 200, .25f * Screen.height, 400, .7f * Screen.height), text, theGuiTextStyle);
		
		//If we are not on the main menu, we need a back button to return to it.
		if (backButton) {
			if (GUI.Button (new Rect (.5f * Screen.width - 100, .8f * Screen.height, 200, .12f * Screen.height), "Back to Main Shop")) {
				main = true;
				backButton = false;
				basicShipMenu = false;
				fighterShipMenu = false;
				stealthShipMenu = false;
				mothershipMenu = false;
				mainShopInt = -1;
				secondaryUpgradeInt = -1;
				text = "";
			}
		}
		
		//If we are on the main menu, show it.
		//Notice that it is only in these few options that we need to declare the backButton bool as true, since otherwise it just holds its value.
		if (main) {
			//The selectionGrid will allow you to choose exactly one of a set of buttons. mainShopInt is initialized to -1 so no level starts out selected.
			int tempInt = GUI.SelectionGrid (new Rect (.2f * Screen.width, .4f * Screen.height, .6f * Screen.width, .4f * Screen.height), mainShopInt, mainUpgradeTypeStrings, mainUpgradeTypeStrings.Length/3);
			//This if statement is used to prevent us from running the remaining logic too often, especially in OnGUI.
			if (tempInt != mainShopInt) {
				switch (tempInt) {
				case 0:
					basicShipMenu = true;
					main = false;
					backButton = true;
					break;
				case 1:
					fighterShipMenu = true;
					main = false;
					backButton = true;
					break;
				case 2:
					stealthShipMenu = true;
					main = false;
					backButton = true;
					break;
				case 3:
					mothershipMenu = true;
					main = false;
					backButton = true;
					break;
				}
				mainShopInt = tempInt;
			}
		} else {
			if (basicShipMenu){
				secondaryUpgradeStrings = basicShipMenuStrings;
			} else if (fighterShipMenu) {

			} else if (stealthShipMenu) {

			} else if (mothershipMenu) {

			}
			int tempInt = GUI.SelectionGrid (new Rect (.2f * Screen.width, .4f * Screen.height, .6f * Screen.width, .4f * Screen.height), secondaryUpgradeInt, secondaryUpgradeStrings, secondaryUpgradeStrings.Length/3);
			//This if statement is used to prevent us from running the remaining logic too often, especially in OnGUI.
			if (tempInt != secondaryUpgradeInt) {
				switch (tempInt) {
				case 0:
					
					break;
				case 1:
					
					break;
				case 2:
					
					break;
				case 3:
					
					break;
				}
				secondaryUpgradeInt = tempInt;
			}
			
		}
	}
}
