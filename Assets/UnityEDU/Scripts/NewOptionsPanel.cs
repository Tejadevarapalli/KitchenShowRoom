using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ComponentObject
{
    public KitchenComponent component;
    public GameObject gameObject;
    public GameObject[] additionalGameObjects;
}

[System.Serializable]
public class ComponentObjectContainers
{
    public Transform parent;
    public GameObject componentObject;
}

[System.Serializable]
public class MaterialObject
{
    public Material material;
    public GameObject gameObject;
}

[System.Serializable]
public class MaterialSet
{
    public MaterialCollection collection;
    public MaterialObject[] materialObjects;
}

public class NewOptionsPanel : MonoBehaviour
{
    public ComponentObject[] componentObjectDefinitions;
    public MaterialSet[] materialSets;
    public GameObject[] optionHolders;
    [Tooltip("This is to handle cases where not all individual options in a row are always needed, currently only applying to knobs and handles.")]
    public GameObject[] hideByDefault;

    private CameraController camScript;

    // Start is called before the first frame update
    void Start()
    {
        camScript = Camera.main.GetComponent<CameraController>();

        CameraController.UpdateSelection += UpdatePanel;

    }
    
    bool MaterialCollectionFound(MaterialCollection collection)
    {
        //foreach (MaterialCollectionHolder holder in materialCollectionHolders)
        //{
        //    if (holder.collection == collection)
        //    {
        //        holder.MaterialCollectionObject.SetActive(true);
        //        return true;
        //    }
        //}
        return false;
    }

    

    private void ShowKitchenComponent(KitchenComponent component)
    {
        foreach (ComponentObject def in componentObjectDefinitions)
        {
            // this was a last minute addition when I realized sometimes more than one thing needs to be toggled on:
            // example: both "Add Knobs" and "Add Pulls" also need the row of metal options available
            if (def.additionalGameObjects.Length > 0)
            {
                foreach (GameObject GO in def.additionalGameObjects)
                {
                    GO.transform.parent.gameObject.SetActive(true);
                    GO.SetActive(true);
                    if (GO.CompareTag("Knobs"))
                    {
                        // set the toggle by whether or not the knobs are visible on this gameObject
                        if (camScript == null)
                            camScript = Camera.main.GetComponent<CameraController>();

                        GameObject currentGO = camScript.selectedObj.gameObject;
                        if (currentGO != null)
                        {
                            for (int i = 0; i < currentGO.transform.childCount; i++)
                            {
                                if (currentGO.transform.GetChild(i).gameObject.CompareTag("Knobs"))
                                {
                                    bool active = currentGO.transform.GetChild(i).gameObject.activeInHierarchy;
                                    Toggle toggle = GO.GetComponent<Toggle>();
                                    if (toggle != null)
                                        toggle.isOn = active;
                                }
                            }
                        }

                    }
                    if (GO.CompareTag("Pulls"))
                    {
                        // set the toggle by whether or not the knobs are visible on this gameObject
                        if (camScript == null)
                            camScript = Camera.main.GetComponent<CameraController>();

                        GameObject currentGO = camScript.selectedObj.gameObject;
                        if (currentGO != null)
                        {
                            for (int i = 0; i < currentGO.transform.childCount; i++)
                            {
                                if (currentGO.transform.GetChild(i).gameObject.CompareTag("Pulls"))
                                {
                                    bool active = currentGO.transform.GetChild(i).gameObject.activeInHierarchy;
                                    Toggle toggle = GO.GetComponent<Toggle>();
                                    if (toggle != null)
                                        toggle.isOn = active;
                                }
                            }
                        }

                    }
                }
            }

            if (component == def.component)
            {
                def.gameObject.transform.parent.gameObject.SetActive(true);
                def.gameObject.SetActive(true);
                if (def.gameObject.CompareTag("Knobs"))
                {
                    // set the toggle by whether or not the knobs are visible on this gameObject
                    if (camScript == null)
                        camScript = Camera.main.GetComponent<CameraController>();

                    GameObject currentGO = camScript.selectedObj.gameObject;
                    if (currentGO != null)
                    {
                        for (int i = 0; i < currentGO.transform.childCount; i++)
                        {
                            if (currentGO.transform.GetChild(i).gameObject.CompareTag("Knobs"))
                            {
                                bool active = currentGO.transform.GetChild(i).gameObject.activeInHierarchy;
                                Toggle toggle = def.gameObject.GetComponent<Toggle>();
                                if (toggle != null)
                                    toggle.isOn = active;
                            }
                        }
                    }
                                                
                }
                if (def.gameObject.CompareTag("Pulls"))
                {
                    // set the toggle by whether or not the knobs are visible on this gameObject
                    if (camScript == null)
                        camScript = Camera.main.GetComponent<CameraController>();

                    GameObject currentGO = camScript.selectedObj.gameObject;
                    if (currentGO != null)
                    {
                        for (int i = 0; i < currentGO.transform.childCount; i++)
                        {
                            if (currentGO.transform.GetChild(i).gameObject.CompareTag("Pulls"))
                            {
                                bool active = currentGO.transform.GetChild(i).gameObject.activeInHierarchy;
                                Toggle toggle = def.gameObject.GetComponent<Toggle>();
                                if (toggle != null)
                                    toggle.isOn = active;
                            }
                        }
                    }

                }
            }
        }
    }

    public void UpdatePanel()
    {
        // disable all options in the panel
        foreach (GameObject go in optionHolders)
            go.SetActive(false);
        foreach (GameObject go in hideByDefault)
            go.SetActive(false);

        GameObject currentObj = camScript.selectedObj.gameObject;
        if (currentObj == null)
            return;

        ObjectData currentData = currentObj.GetComponent<ObjectData>();
        if (currentData == null)
            return;

        // show only the options needed by the currently selected object
        for (int i = 0; i < currentData.components.Count; i++)
            ShowKitchenComponent(currentData.components[i]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
