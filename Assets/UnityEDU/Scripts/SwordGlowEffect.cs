using UnityEngine;
using UnityEngine.VR;

public class SwordGlowEffect : MonoBehaviour 
{
	const string rightTriggerAxis = "SecondaryIndexTrigger";	//Input mapping for Oculus Touch. Would need remapped for Vive
	const string leftTriggerAxis = "PrimaryIndexTrigger";		//Input mapping for Oculus Touch. Would need remapped for Vive

	Animator anim;								//A reference to the animator component
	bool isRightHand;							//Is this sword in the right hand?

	void Awake () 
	{
		if (!UnityEngine.XR.XRSettings.enabled) 
		{
			Destroy (this);
			return;
		}

		//Get a reference to the animator component on the sword child object
		anim = GetComponentInChildren<Animator> ();

		//Determine if this is the right hand or not by examining the parent VRObjectTracking script
		UnityEngine.XR.XRNode node = transform.parent.GetComponent<VRObjectTracking> ().node;
		if (node == UnityEngine.XR.XRNode.RightHand)
			isRightHand = true;
	}

	void Update()
	{
		//Look for inputs on the left and right hands. If there is input and they match
		//the hand this sword is in, then play the charge animation. Otherwise stop the
		//charge animation. Note that the Oculus Touch treats the trigger as an axis and so we
		//use GetAxisRaw(). The Vive treats the trigger as a button and you would instead
		//need to use GetButtonDown()
		if ((Input.GetAxisRaw (rightTriggerAxis) > 0f && isRightHand) ||
			(Input.GetAxisRaw(leftTriggerAxis) > 0f && !isRightHand))
			anim.SetBool ("Charged", true);
		else
			anim.SetBool ("Charged", false);
	}
}
