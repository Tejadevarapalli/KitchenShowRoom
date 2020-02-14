//This script handles pausing and unpausing the game. It is responsible for managing the game's timescale while paused
//as well as the player's hands while paused. 

using UnityEngine;

public class PauseManager : MonoBehaviour 
{
	[SerializeField] bool startPaused = true;		//Should the game be paused to start?
	[SerializeField] GameObject pauseScreen;		//A reference to the UI object that shows the paused screen
	[SerializeField] LayerMask pauseCullingMask;	//A culling mask to limit what the camera can see when paused

	GM_HandModule hands;							//A reference to the player's hand module
	Camera mainCamera;								//A reference to the main camera
	int prevCullingMask;							//A variable to contain any previous culling mask
	bool isPaused;									//Is the game currently paused?
	float prevTimeScale;							//A variable to contain the previous time scale


	void Start()
	{
		//Get references to the hands module and HMD
		hands = GameManager.instance.hands;
		mainCamera = GameManager.instance.HMD;

		//If the game should start paused, pause the game
		if (startPaused)
			TogglePaused (true);
	}

	void Update () 
	{
		//Look for the player to press "Button2". If they do, toggle the paused state
		if (Input.GetButtonDown ("Button2") || Input.GetButtonDown ("Fire2"))
			TogglePaused (!isPaused);
	}

	public void TogglePaused(bool shouldPause)
	{
		//This prevents us from pausing a game that is already paused
		if (isPaused == shouldPause)
			return;

		//Record whether the game is now paused or not
		isPaused = shouldPause;

		//Toggle the paused screen
		pauseScreen.SetActive (isPaused);

		//If the game is paused...
		if (isPaused)
		{
			//...switch the hands to controllers...
			hands.SwitchToControllers ();
			//...record the current culling mask and then set the camera's culling mask...
			prevCullingMask = mainCamera.cullingMask;
			mainCamera.cullingMask = pauseCullingMask.value;
			//...finally record the current timescale and set the timescale to 0
			prevTimeScale = Time.timeScale;
			Time.timeScale = 0f;
		}
		//...Otherwise...
		else
		{
			//...switch the hands to swords...
			hands.SwitchToSwords ();
			//...and set the culling mask and timescale back to normal
			mainCamera.cullingMask = prevCullingMask;
			Time.timeScale = prevTimeScale;
		}
	}
}
