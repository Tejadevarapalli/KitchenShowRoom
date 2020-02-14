//This script disables an object after a delay

using UnityEngine;

public class TimedDisable : MonoBehaviour 
{
	[SerializeField] float delay = 5f;	//The amount of time before the object is disabled

	void OnEnable()
	{
		//Cancel any previous scheduled Invoke calls
		CancelInvoke ();
		//Schedule a call to Disable() after a delay
		Invoke ("Disable", delay);
	}

	void Disable()
	{
		//Disable the game object
		gameObject.SetActive (false);
	}
}
