using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObjectUpdater : MonoBehaviour
{
    public Material material;
    private CameraController camControl;

    private void Awake()
    {
        camControl = FindObjectOfType<CameraController>();
    }


    public void MeshSwitch()
    {
        if (camControl == null)
        {
            camControl = FindObjectOfType<CameraController>();
        }
        GameObject currentGO = camControl.selectedObj.gameObject;
        ObjectData currentData = currentGO.GetComponent<ObjectData>();
        TMPro.TMP_Text buttonName = GetComponentInChildren<TMPro.TMP_Text>();
        
        if (CompareTag("Kitchen"))
        {
            foreach (GameObject sink in currentData.sinkOptions)
            {
                if (buttonName.text == sink.name)
                {
                    sink.SetActive(true);
                }
                else
                {
                    sink.SetActive(false);
                }
            }
        }

        if (CompareTag("Door"))
        {
            foreach (GameObject door in currentData.doorOptions)
            {
                if (buttonName.text == door.name)
                {
                    door.SetActive(true);
                }
                else
                {
                    door.SetActive(false);
                }
            }
        }

        if (CompareTag("Drawer"))
        {
            foreach (GameObject drawer in currentData.drawerOptions)
            {
                if (buttonName.text == drawer.name)
                {
                    drawer.SetActive(true);
                }
                else
                {
                    drawer.SetActive(false);
                }
            }

        }

        if (CompareTag("ApplianceOpt"))
        {
            foreach (GameObject washer in currentData.applianceOptions)
            {
                if (buttonName.text == washer.name)
                {
                    washer.SetActive(true);
                }
                else
                {
                    washer.SetActive(false);
                }
            }
        }
    }

    public void VisibilityToggle(bool value)
    {
        if (camControl == null)
        {
            camControl = FindObjectOfType<CameraController>();
        }

        GameObject currentGO = camControl.selectedObj.gameObject;
        ObjectData currentData = currentGO.GetComponent<ObjectData>();
        GameObject toggledObject = null;

        if (gameObject.CompareTag("Knobs"))
        {
            toggledObject = currentData.knobs;

        }
        else if (gameObject.CompareTag("Pulls"))
        {
            toggledObject = currentData.pulls;
        }

        if (toggledObject != null)
        {
            toggledObject.SetActive(value);
        }
    }

    public void UpdateMaterial()
    {
        if (camControl == null)
        {
            camControl = FindObjectOfType<CameraController>();
        }
        GameObject currentGO = camControl.selectedObj.gameObject;
        ObjectData currentData = currentGO.GetComponent<ObjectData>();
        string currentTag = tag;
        Debug.Log("ObjectUpdater MaterialSwitch CurrentObject name: " + currentGO.name);
        Debug.Log("ObjectUpdater MaterialSwitch Debug tag: " + tag);

        List<GameObject> currentCollection = new List<GameObject>();
        List<Material> currentOptions = new List<Material>();
        TMPro.TMP_Text buttonName = GetComponentInChildren<TMPro.TMP_Text>();

        switch (currentTag)
        {
            /*Get all granite objects in the scene. This is the only material section that 
            * updates the material across all objects, as countertops are generally single slabs */
            case "Granite":
                currentOptions = currentData.counterMat;
                List<GameObject> tempGranite = new List<GameObject>(GameObject.FindGameObjectsWithTag("Granite"));
                foreach (var granite in tempGranite)
                {
                    if (granite.GetComponent<ObjectUpdater>() == null)
                    {
                        currentCollection.Add(granite);
                    }
                }

                break;

            case "Door":
                currentOptions = currentData.woodMat;
                foreach (var woodpart in currentData.doorOptions)
                {
                    foreach (Transform child in woodpart.transform)
                    {
                        currentCollection.Add(child.gameObject);
                    }
                }

                break;

            case "Drawer":
                currentOptions = currentData.woodMat;
                foreach (var woodPart in currentData.drawerOptions)
                {
                    foreach (Transform child in woodPart.transform)
                    {
                        currentCollection.Add(child.gameObject);
                    }
                }

                break;

            case "Wood":
                currentOptions = currentData.woodMat;
                foreach (var body in currentData.woodBase)
                {
                    currentCollection.Add(body);
                }

                break;

            case "Pulls":
                currentOptions = currentData.accessoryMat;
                foreach (Transform child in currentData.pulls.transform)
                {
                    currentCollection.Add(child.gameObject);
                }

                break;

            case "Knobs":
                currentOptions = currentData.accessoryMat;
                foreach (Transform child in currentData.knobs.transform)
                {
                    currentCollection.Add(child.gameObject);
                }

                break;

            case "Accessory":
                // pulls and knobs
                // this search is necessary in case an object doesn't have its pulls and/or knobs set in the Inspector
                currentOptions = currentData.accessoryMat;
                if (currentData.pulls == null)
                {
                    for (int i = 0; i < currentGO.transform.childCount; i++)
                    {
                        if (currentGO.transform.GetChild(i).gameObject.CompareTag("Pulls"))
                            currentData.pulls = currentGO.transform.GetChild(i).gameObject;
                    }
                }
                if (currentData.pulls != null)
                {
                    foreach (Transform child in currentData.pulls.transform)
                    {
                        currentCollection.Add(child.gameObject);
                    }
                }
                if (currentData.knobs == null)
                {
                    for (int i = 0; i < currentGO.transform.childCount; i++)
                    {
                        if (currentGO.transform.GetChild(i).gameObject.CompareTag("Knobs"))
                            currentData.knobs = currentGO.transform.GetChild(i).gameObject;
                    }
                }
                if (currentData.knobs != null)
                {
                    foreach (Transform child in currentData.knobs.transform)
                    {
                        currentCollection.Add(child.gameObject);
                    }
                }              
                break;
            default:
                // handle wood options. It seems that the untagged items all use wood.

                currentOptions = currentData.woodMat;
                foreach (var body in currentData.woodBase)
                {
                    currentCollection.Add(body);
                }

                foreach (var woodpart in currentData.doorOptions)
                {
                    foreach (Transform child in woodpart.transform)
                    {
                        currentCollection.Add(child.gameObject);
                    }
                }

                foreach (var woodPart in currentData.drawerOptions)
                {
                    foreach (Transform child in woodPart.transform)
                    {
                        currentCollection.Add(child.gameObject);
                    }
                }

                //Debug.Log("Warning: Selected an object without a defined tag!");
                break;
        }

        foreach (var currentObj in currentCollection)
        {
            Renderer adjustGO = currentObj.GetComponent<Renderer>();
            if (adjustGO != null)
            adjustGO.sharedMaterial = material;
        }
    }
}

