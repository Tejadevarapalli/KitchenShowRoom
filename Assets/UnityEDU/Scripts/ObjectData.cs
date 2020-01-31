using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour {

    
    public List<KitchenComponent> components = new List<KitchenComponent>();
    public List <GameObject> woodBase = new List<GameObject>();
    public List<GameObject> drawerOptions = new List<GameObject>();
    public List<GameObject> doorOptions = new List<GameObject>();
    public List<GameObject> sinkOptions = new List<GameObject>();
    public List<GameObject> applianceOptions = new List<GameObject>();
    public List<Material> counterMat = new List<Material>();
    public List<Material> woodMat = new List<Material>();
    public List<Material> applianceMat = new List<Material>();
    public List<Material> accessoryMat = new List<Material>();
    public List<Material> surfaceMat = new List<Material>();

    [HideInInspector]
    public GameObject knobs;
    [HideInInspector]
    public GameObject pulls;



    private void Awake()
    {
        for (int i = 0; i < components.Count; i++)
        {
            GatherObjectData(components[i]);
        }
    }

    void GatherObjectData(KitchenComponent currentComp)
    {
       
        switch (currentComp.componentType)
        {
            case KitchenComponent.ComponentType.Accessory:

                foreach (Transform door in transform)
                {
                    if (door.gameObject.CompareTag("Knobs"))
                    {
                        knobs = door.gameObject;

                    } else if (door.gameObject.CompareTag("Pulls"))
                    {
                        pulls = door.gameObject;
                    }
                }
                for (int i = 0; i < currentComp.materialList.Count; i++)
                {
                    List<Material> newMats = currentComp.materialList[i].materialList;
                    foreach (Material mat in newMats)
                    {
                        accessoryMat.Add(mat);
                    }
                }
                break;
            case KitchenComponent.ComponentType.Appliance:

                for (int i = 0; i < currentComp.materialList.Count; i++)
                {
                    List<Material> newMats = currentComp.materialList[i].materialList;
                    foreach (Material mat in newMats)
                    {
                        applianceMat.Add(mat);
                    }
                }

                foreach (Transform appliance in transform)
                {
                    if (appliance.gameObject.CompareTag("ApplianceOpt"))
                    {
                        applianceOptions.Add(appliance.gameObject);
                    }
                }
                break;
            case KitchenComponent.ComponentType.Countertop:

                for (int i = 0; i < currentComp.materialList.Count; i++)
                {
                    List<Material> newMats = currentComp.materialList[i].materialList;
                    foreach (Material mat in newMats)
                    {
                        counterMat.Add(mat);
                    }
                } 
                break;
            case KitchenComponent.ComponentType.Doors:


                foreach (Transform door in transform)
                {
                    if (door.gameObject.CompareTag("Door"))
                    {
                        doorOptions.Add(door.gameObject);
                    }
                }

                if (woodMat.Count == 0)
                {
                    for (int i = 0; i < currentComp.materialList.Count; i++)
                    {
                        List<Material> newMats = currentComp.materialList[i].materialList;
                        foreach (Material mat in newMats)
                        {
                            woodMat.Add(mat);
                        }
                    }
                }
                break;

            case KitchenComponent.ComponentType.Drawers:

                foreach (Transform drawer in transform)
                {
                    if (drawer.gameObject.CompareTag("Drawer"))
                    {
                        drawerOptions.Add(drawer.gameObject);
                    }
                }

                if (woodMat.Count == 0)
                {
                    for (int i = 0; i < currentComp.materialList.Count; i++)
                    {
                        List<Material> newMats = currentComp.materialList[i].materialList;
                        foreach (Material mat in newMats)
                        {
                            woodMat.Add(mat);
                        }
                    }
                }
                break;
            case KitchenComponent.ComponentType.Surface:
                for (int i = 0; i < currentComp.materialList.Count; i++)
                {
                    List<Material> newMats = currentComp.materialList[i].materialList;
                    foreach (Material mat in newMats)
                    {
                        surfaceMat.Add(mat);
                    }
                }
                break;
            default:
                Debug.Log("This wasn't assigned");
                break;
        }
    }

}
