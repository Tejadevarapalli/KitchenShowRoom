//This script manages the various Object Pools used by the game. Pooling 
//objects is a great way to reduce memory consumption and improve game
//efficiency

using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour 
{
	public static ObjectPoolManager instance;	//A singleton reference to the Object Pool Manager

	public ObjectPool[] pools;
	Dictionary<string, ObjectPool> _pools;


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	void Start () 
	{
		_pools = new Dictionary<string, ObjectPool> ();

		for (int i = 0; i < pools.Length; i++)
		{
			//ObjectPool pool = new ObjectPool();
			//pool.Init (pools [i].pooledObject, pools [i].size, transform);
			pools[i].Init(transform);
			_pools.Add (pools [i].pooledObject.name, pools[i]);
		}	
	}

	public GameObject GetObject(string ID, bool activate = false, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
	{
		if (!_pools.ContainsKey (ID))
		{
			VRLog.Log ("Pool: " + ID + " does not exist");
			return null;
		}

		GameObject obj = _pools [ID].GetObject ();

		if (obj == null)
		{
			obj = Instantiate (_pools [ID].pooledObject);
			VRLog.Log (ID + " pool not big enough. Object instantiated");
		}
			
		obj.transform.position = position;
		obj.transform.rotation = rotation;

		if(activate)
			obj.SetActive (true);

		return obj;
	}
}
