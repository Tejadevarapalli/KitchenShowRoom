using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PooledItem
{
    public GameObject objectToPool;
    public int amtToPool;
    public bool dynamicExpand;

}

public class PoolGenerator : MonoBehaviour {

    public static PoolGenerator instance;

    public List<PooledItem> itemsToPool;
    public Transform parentObj;
    public List<GameObject> pooledObjects;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (PooledItem item in itemsToPool)
        {
            for (int i = 0; i < item.amtToPool; i++)
            {
                GameObject GO = Instantiate(item.objectToPool, parentObj);
                GO.SetActive(false);
                pooledObjects.Add(GO);
            }

        }
    }

    public GameObject GetPooledObject (string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].CompareTag(tag)) 
            {
                return pooledObjects[i];
            }
        }
        foreach (PooledItem item in itemsToPool)
        {
            if(item.objectToPool.CompareTag(tag))
            {
                if (item.dynamicExpand)
                {
                    GameObject GO = Instantiate(item.objectToPool, parentObj);
                    GO.SetActive(false);
                    pooledObjects.Add(GO);
                    return GO;
                }

            }
        }
        return null;
    }

}
