using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{

	private Vector2 initialMousePos; //Track position of initial contact.
	private Vector2 finalMousePos;	//Track position of final contact.
	private Vector2 movingMousePos;	//Track current position during motion.
	private Vector2 deltaMovingMousePos;	//Track change in position since previous frame.
	private bool clickDown;	//Is the mouse down/screen being touched?
	private bool movingClick;	//Is the mouse moving/screen being swiped?
	private bool clickUp;	//Is the mouse up/screen not being touched?
	private bool clickStarted;	//Has a click/touch just started?
	private bool clickEnded;	//Has a click/touch just ended?
	private bool clickMoved;	//Has a click/touch just moved while down?
	private bool onTouchScreen;	//Are we on a touch-screen device?
	private bool trigger; //Have we input the trigger command for special behavior?

	public void Start ()
	{

		clickDown = false; //Initially the mouse is up/screen is not being touched.
		movingClick = false;	//Initially there is no click/touch, so it is not moving.
		clickUp = true;	//Initially the mouse is up/screen is not being touched.
		clickStarted = false;	//No touch/click has started.
		clickEnded = false;		//No touch/click has ended.
		clickMoved = false;		//No touch/click has moved.
		onTouchScreen = false;	//Assume we are not on a touchscreen.
		trigger = false;
		//If we are, modify that assumption.
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			onTouchScreen = true;
		}
	}
	
	void Update ()
	{
		if (!onTouchScreen) {
			//If we thought the mouse was up but now it is down, it was just clicked down. So a click started.
			if (clickUp && Input.GetMouseButton (0)) {
				clickEnded = false;
				clickDown = true;
				clickUp = false;	
				clickStarted = true;
				initialMousePos = Input.mousePosition;
			//If we thought the mouse was just clicked down and it is still down in the next frame, it is moving.
			} else if (clickDown && Input.GetMouseButton (0)) {
				clickMoved = true;
				clickDown = false;
				movingClick = true;
				movingMousePos = Input.mousePosition;
			//If the mouse is moving and still down, it is still moving, so update the position and note the change in position.
			} else if (movingClick && Input.GetMouseButton (0)) {
				clickMoved = true;
				deltaMovingMousePos = (Vector2)Input.mousePosition - movingMousePos;
				movingMousePos = Input.mousePosition;
			//If the mouse was moving but is now up, the drag has ended.
			} else if (movingClick && !Input.GetMouseButton (0)) {
				movingClick = false;
				clickUp = true;
				clickEnded = true;
				finalMousePos = Input.mousePosition;
			//If the mouse was down but is now up, the click has ended.
			} else if (clickDown && !Input.GetMouseButton (0)) {
				clickDown = false;
				clickUp = true;
				clickEnded = true;
				finalMousePos = initialMousePos;
			}

			if(Input.GetKeyDown("space")){
				trigger = true;
			}
		} else {
			//If a touch began, mark it down.
			if (Input.GetTouch (0).phase == TouchPhase.Began) {	
				clickEnded = false;
				clickStarted = true;
				clickDown = true;
				initialMousePos = Input.GetTouch (0).position;
			//If a swipe began, mark it down.
			} else if (clickDown && Input.GetTouch (0).phase == TouchPhase.Moved) {
				clickMoved = true;
				clickDown = false;
				movingMousePos = Input.GetTouch (0).position;
			//If a swipe continued, update the position info.
			} else if (!clickDown && Input.GetTouch (0).phase == TouchPhase.Moved) {
				clickMoved = true;
				deltaMovingMousePos = (Vector2)Input.GetTouch (0).position - movingMousePos;
				movingMousePos = Input.GetTouch (0).position;
			//If a touch ended, mark it down.
			} else if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				finalMousePos = Input.GetTouch (0).position;
				clickEnded = true;
			}

			if(Input.acceleration.sqrMagnitude > 5){
				trigger = true;
			}
		}
		


	}

	public bool Began ()
	{
		if (clickStarted) {
			clickStarted = false;
			return true;
		}
		return false;
	}

	public bool Moved ()
	{
		if (clickMoved) {
			clickMoved = false;
			return true;
		}
		return false;
	}
	
	public bool Ended ()
	{
		if (clickEnded) {
			clickEnded = false;
			return true;
		}
		return false;
	}

	public Vector2 startPos ()
	{
		return initialMousePos;
	}

	public Vector2 deltaPos ()
	{
		return deltaMovingMousePos;
	}

	public Vector2 endPos ()
	{
		return finalMousePos;
	}

	public Vector2 currentDragPos ()
	{
		return movingMousePos;
	}

	public Vector2 currentHoverPos ()
	{
		return Input.mousePosition;
	}

	public bool isTrigger(){
		if(trigger){
			trigger = false;
			return true;
		}
		return false;
	}

	public void setTrigger(bool b){
		trigger = b;
	}

	public void setEnded(bool b){
		clickEnded = b;
	}
	public void setBegan(bool b){
		clickStarted = b;
	}
	public void setMoved(bool b){
		clickMoved = b;
	}

	
}
