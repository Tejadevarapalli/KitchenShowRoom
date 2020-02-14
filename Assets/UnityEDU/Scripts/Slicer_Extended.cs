//This script handles the slicing of melee drone. It extends the code from the Turbo Slicer API

using UnityEngine;
using NobleMuffins.TurboSlicer;

public class Slicer_Extended : MonoBehaviour 
{
	const string cutPoolName = "MeleeCut";			//Name of the melee cut prefab in the Object Pool		

	[SerializeField] float sliceCoolDown = .1f;		//Cooldown period to prevent the sword from making multiple cuts too quickly
	[SerializeField] Transform[]  planeDefiner;		//The plane of the sword slice

	bool onCD;										//Is the sword currently on cooldown?

		
	void OnTriggerEnter(Collider other)
	{
		//If the sword in on cooldown, exit
		if (onCD)
			return;

		//Try to get the CustomSliceHandler component on the collided object. If it doesn't exist, exit
		CustomSliceHandler victim = other.gameObject.GetComponent<CustomSliceHandler> ();
		if (victim == null)
			return;

		//Get a melee cut object at the position of the sliced object
		ObjectPoolManager.instance.GetObject (cutPoolName, true, other.transform.position);

		//Slice the collided object using Turbo Slicer
		TurboSlicerSingleton.Instance.SliceByTriangle (victim.gameObject, GetPlane(), true);

		//Put the sword on cooldown and schedule a call to the Cooldown() method
		onCD = true;
		Invoke ("Cooldown", sliceCoolDown);
	}

	void Cooldown()
	{
		//Take the sword off of cooldown
		onCD = false;
	}


	//This method defines a new plane based off of the three points of the sword's cutting plane
	Vector3[] GetPlane()
	{
		Vector3[] plane = new Vector3[3];
		for (int i = 0; i < 3; i++)
		{
			plane [i] = planeDefiner [i].position;
		}

		return plane;
	}
}
