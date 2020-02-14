//This script tracks the player's body position to the HMD position while ignoring the 
//rotation of the HMD

using UnityEngine;

public class BodyToHeadTracking : MonoBehaviour 
{
	Transform head;	//The player's head (HMD)


	void Start()
	{
		//Get a reference to the player's head
		head = GameManager.instance.HMD.transform;
	}

	void LateUpdate()
	{
		//Move this body to the same position as the HMD
		transform.position = head.position;
	}
}
