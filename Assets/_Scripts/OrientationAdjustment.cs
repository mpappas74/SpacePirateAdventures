using UnityEngine;
using System.Collections;

public class OrientationAdjustment : MonoBehaviour {
	//Really simple script that just enforces LandscapeLeft orientation.
	//Also possible to have it detect the screen orientation, but since portrait looks horrible, it's better to just enforce.	

	void Awake()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}
	
}
