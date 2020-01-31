using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script has been added to each UI button prefab. Using the button's assigned tag, it determines what
//material the button is displaying, and what object it should switch the material on.
public class ObjectUpdater : MonoBehaviour {

    private CameraController camControl;

    private void Awake()
    {
        camControl = Camera.main.GetComponent<CameraController>();
    }

    public void VisibilityToggle ()
    {
        GameObject currentGO = camControl.selectedObj.gameObject;
        ObjectData currentData = currentGO.GetComponent<ObjectData>();
        GameObject toggledObject = null;

        if (gameObject.CompareTag("Knobs"))
        {
            toggledObject = currentData.knobs;

        } else if (gameObject.CompareTag("Pulls"))
        {
            toggledObject = currentData.pulls;
        }

        if (toggledObject != null && toggledObject.activeInHierarchy)
        {
            toggledObject.SetActive(false);
        }
        else
        {
            toggledObject.SetActive(true);
        }
    }

    public void MeshSwitch ()
    {
        if (camControl == null)
        {
            Debug.Log("CamControl is null.");
            camControl = FindObjectOfType<CameraController>();
        }
        GameObject currentGO = camControl.selectedObj.gameObject;
        ObjectData currentData = currentGO.GetComponent<ObjectData>();
        TMPro.TMP_Text buttonName = GetComponentInChildren<TMPro.TMP_Text>();

        if (CompareTag("Door"))
        {
            foreach (GameObject door in currentData.doorOptions)
            {
                if (buttonName.text == door.name)
                {
                    door.SetActive(true);
                } else
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

    public void MaterialSwitch ()
    {
        if (camControl == null)
        {
            Debug.Log("CamControl is null.");
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
            default:
                // handle wood options. It seems that the untagged items all use wood.

                currentOptions = currentData.woodMat;
                foreach (var body in currentData.woodBase)
                {
                    currentCollection.Add(body);
                }

                currentOptions = currentData.woodMat;
                foreach (var woodpart in currentData.doorOptions)
                {
                    foreach (Transform child in woodpart.transform)
                    {
                        currentCollection.Add(child.gameObject);
                    }
                }

                //Debug.Log("Warning: Selected an object without a defined tag!");
                break;
        }

        Material adjustedMat = null;
        foreach (var mat in currentOptions)
        {
            if (mat.name == buttonName.text)
            {
                adjustedMat = mat;

            }
        }

        foreach (var currentObj in currentCollection)
        {
            Renderer adjustGO = currentObj.GetComponent<Renderer>();
            adjustGO.sharedMaterial = adjustedMat;
        }
    }
}
