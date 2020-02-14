//This script destroys the game object it is on after a period of time

using UnityEngine;

public class Destroy : MonoBehaviour 
{
	public float timeToLive = 3f;	//The time before destroying this object


	void Start () 
	{
		//Destory the object after the set amount of time
		Destroy (gameObject, timeToLive);
	}
}
