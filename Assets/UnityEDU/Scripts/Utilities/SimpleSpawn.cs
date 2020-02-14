
using UnityEngine;

public class SimpleSpawn : MonoBehaviour 
{
	public GameObject prefab;

	GameObject spawnedObject;


	void Start () 
	{
		Spawn ();
	}

	void Update()
	{
		if (spawnedObject == null)
			Spawn ();
	}

	public void Spawn()
	{
		spawnedObject = Instantiate (prefab, transform.position, transform.rotation) as GameObject;
	}
}
