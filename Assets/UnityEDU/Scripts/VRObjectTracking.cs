//This script allows for 3D input tracking using Unity's built in tracking system

using UnityEngine;
using UnityEngine.VR;

public class VRObjectTracking : MonoBehaviour 
{
    public UnityEngine.XR.XRNode node;	//The device type that this game object represents

	void Awake()
	{
		//If VR isn't enabled, don't try to track this object
		if (!UnityEngine.XR.XRSettings.enabled)
			Destroy(this);
	}


	void Update()
	{
		//Update the position and rotation of this game object
		transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition (node);
		transform.localRotation = UnityEngine.XR.InputTracking.GetLocalRotation (node);
	}
}
