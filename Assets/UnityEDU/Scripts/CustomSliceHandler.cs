//This script manages the actions performed on a melee drone after it has been sliced with the sword. This code is dependent on the
//Turbo Slicer asset package

using UnityEngine;

public class CustomSliceHandler : AbstractSliceHandler 
{
	[SerializeField] float separationForce = 1f;	//Amount of force applied to the sliced fragments
	[SerializeField] float upwardForce = .5f;		//Amount of upward force applied to sliced fragments
	[SerializeField] float separationRadius = 1f;	//Radius of force used to separate the fragments


	//This method is a callback from the Turbo Slicer API. It will handle the newly created fragments
	public override void handleSlice( GameObject[] results )
	{
		//Attempt to access a MeleeDrone script on this object. If one exists, tell
		//it that it has been sliced
		MeleeDrone melee = GetComponent<MeleeDrone> ();
		if (melee != null)
			melee.Sliced ();

		//Iterate through all the new pieces and...
		for (int i = 0; i < results.Length; i++)
		{
			//...get the fragments rigidbody...
			Rigidbody rb = results [i].GetComponent<Rigidbody> ();
			if (rb != null)
			{
				//...if the fragment has a rigidbody enable gravity and use an explosion force to "pop" them apart
				rb.useGravity = true;
				rb.AddExplosionForce (separationForce, transform.position, separationRadius, upwardForce, ForceMode.Impulse);
			}

			//Finally, get the mesh collider of the fragment and set it to the newly created fragment mesh. NOTE: this could
			//be problematic if slicing complex meshes. In this case, however, it will perform well
			Mesh mesh = results [i].GetComponent<MeshFilter> ().mesh;
			results [i].GetComponent<MeshCollider> ().sharedMesh = mesh;
		}	
	}
}
