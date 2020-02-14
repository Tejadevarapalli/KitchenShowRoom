//This script contains the ObjectPool class which represents a single object pool. The Object
//Pool Manager will contain several of these object pools (one for each type of object to pool)

using UnityEngine;

[System.Serializable]
public class ObjectPool 
{
	public GameObject pooledObject;		//A reference to the game object that will be pooled
	public int maxSize;					//The number of objects to pool

	GameObject[] pool;					//The array containing the pool of objects

	//This method is used to initialize the object pool
	public void Init(Transform parent)
	{
		//Initialize the pool array
		pool = new GameObject[maxSize];
		for (int i = 0; i < maxSize; i++)
		{
			//Loop through the array and fill it with objects
			pool [i] = GameObject.Instantiate (pooledObject) as GameObject;
			pool [i].transform.parent = parent;
			pool [i].SetActive (false);
		}
	}

	//This method is called by the pool manager
	public GameObject GetObject()
	{
		//Loop through the pool and return the first inactive object
		for (int i = 0; i < pool.Length; i++)
			if (!pool [i].activeSelf)
				return pool [i];

		//If no inactive objects were found, return null
		return null;
	}
}
