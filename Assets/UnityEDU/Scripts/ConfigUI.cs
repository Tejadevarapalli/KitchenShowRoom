using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Added in to allow access to UI components!

public class ConfigUI : MonoBehaviour {

    /*
     This script populates the UI panel located in the ScreenSpaceCanvas. This script gets the seleced object from the 
     main camera and from it, parses the data held in the object's ObjectData script. All of the UI components (panels,
     buttons, etc) are pulled from the PoolGenerator script.
    */

    private CameraController camScript;
    
    public Image infoPanel;
    
    public GameObject selectionIcon;
    public bool panelActive = false;
    private Animator anim;
    private AutoFocus af;
    private bool autoFocus;
    public ToggleVisibility panelToggle;

    private void Start()
    {
        camScript = Camera.main.GetComponent<CameraController>();
        af = FindObjectOfType<AutoFocus>();
        if (af != null)
        {
            autoFocus = true; 
        }

        CameraController.UpdateSelection += UpdatePosition;
        CameraController.UpdateSelection += UpdatePanel;
        selectionIcon.SetActive(false);
       anim = infoPanel.gameObject.GetComponent<Animator>();
        if (panelToggle == null)
            Debug.LogError("Panel Toggle needs to be set (to the button that toggles the visibility of the options panel, called ShowHideButton in scene KitchenTypeA)!");
    }

    //Move the worldspace UI icon to the selected object
    private void UpdatePosition ()
    {
        Transform newPos = camScript.selectedObj.GetComponentInChildren<UIPositioner>().transform;
        transform.position = newPos.position;
        selectionIcon.SetActive(true);
        infoPanel.enabled = true;
    }

    //Where all the panel generation work happens!
    public void UpdatePanel ()
    {
        return; // this is now handled in NewOptionsPanel.
        GameObject currentObj = camScript.selectedObj.gameObject;
        ObjectData currentData = currentObj.GetComponent<ObjectData>();
        string setTag = null;

        //Remove old panels
        foreach (GameObject pooledObject in PoolGenerator.instance.pooledObjects)
        {
            if (pooledObject.CompareTag("CompPanel"))
            {
                PanelManager panelData = pooledObject.GetComponent<PanelManager>();
                GameObject visibilityToggle = panelData.visibilityOption.gameObject;
                visibilityToggle.SetActive(false);
            }
            pooledObject.SetActive(false);
        }

        if (currentData == null)
            return;

        //Figure out how many panels need to be added and enable them. 
        for (int i = 0; i < currentData.components.Count; i++)
        {
            
            GameObject newPanel = PoolGenerator.instance.GetPooledObject("CompPanel");
            if (newPanel != null)
            {
                newPanel.SetActive(true);
                PanelManager panelData = newPanel.GetComponent<PanelManager>();
                panelData.componentName.text = currentData.components[i].name;

                //Locate the parent object for the child rows.
                Transform panelGroup = null;

                foreach (Transform child in newPanel.transform)
                {
                    if (child.CompareTag("OptionArea"))
                    {
                        panelGroup = child;
                    }
                }

                //Add the child rows

                //Add rows for material collections
                for (int j = 0; j < currentData.components[i].materialList.Count; j++)
                {
                    //Add the new row
                    GameObject childPanel = PoolGenerator.instance.GetPooledObject("OptionRow");
                    childPanel.SetActive(true);
                    childPanel.transform.SetParent(panelGroup, false);
                   
                    //Name the new row
                    PanelManager childData = childPanel.GetComponent<PanelManager>();
                    childData.componentName.text = currentData.components[i].materialList[j].name;

                    //Add material swatches
                    foreach (Material mat in currentData.components[i].materialList[j].materialList)
                    {
                        GameObject swatch = PoolGenerator.instance.GetPooledObject("MatSwatch");
                        swatch.SetActive(true);
                        swatch.transform.SetParent(childData.optionArea.transform, false);

                        //Name the swatch
                        var swatchText = swatch.GetComponentInChildren<TMPro.TMP_Text>();
                        swatchText.text = mat.name;

                        //Add the swatch image
                        var swatchImage = swatch.GetComponent<Image>();
                        foreach (var matSprite in currentData.components[i].materialList[j].swatchList)
                        {
                            if (matSprite.name == swatchText.text)
                            {
                                swatchImage.sprite = matSprite;
                                break;
                            }
                        }

                        if (currentData.components[i].componentType == KitchenComponent.ComponentType.Countertop)
                        {
                            swatch.tag = "Granite";
                        }

                        if (currentData.components[i].componentType == KitchenComponent.ComponentType.Drawers)
                        {
                            swatch.tag = "Drawer";
                        }

                        if (currentData.components[i].componentType == KitchenComponent.ComponentType.Doors)
                        {
                            swatch.tag = "Door";
                        }

                        if (currentData.components[i].componentType == KitchenComponent.ComponentType.Accessory)
                        {
                            if (currentData.components[i].identityType == KitchenComponent.AccessoryType.knob)
                            {
                                swatch.tag = "Knobs";
                            } else if (currentData.components[i].identityType == KitchenComponent.AccessoryType.pull)
                            {
                                swatch.tag = "Pulls";
                            }
                        }

                        if (currentData.components[i].componentType == KitchenComponent.ComponentType.Surface)
                        {
                         
                            if (currentData.components[i].identityType == KitchenComponent.AccessoryType.baseWood)
                            {
                                swatch.tag = "Wood";
                            }
                            else
                            {
                                swatch.tag = "Surface";
                            }
                        }

                        if (currentData.components[i].componentType == KitchenComponent.ComponentType.Appliance)
                        {
                            swatch.tag = "Appliance";
                        }
                    }

                }
                //Add row for swappable mesh options
                if (currentData.components[i].hasSwappableMeshes == KitchenComponent.HasSwappableMeshes.yes)
                {
                    GameObject swapPanel = PoolGenerator.instance.GetPooledObject("OptionRow");
                    swapPanel.SetActive(true);
                    swapPanel.transform.SetParent(panelGroup);
                    PanelManager swapData = swapPanel.GetComponent<PanelManager>();

                    //Determine what kind of swappable mesh there is and how to customize the components
                    switch (currentData.components[i].componentType)
                    {
                        case KitchenComponent.ComponentType.Doors:
                            swapData.componentName.text = "Door Styles";

                            foreach (GameObject door in currentData.doorOptions)
                            {
                                GameObject styleButton = PoolGenerator.instance.GetPooledObject("StyleSwatch");
                                styleButton.SetActive(true);                                
                                styleButton.transform.SetParent(swapData.optionArea.transform, false);
                                styleButton.tag = "Door";
                                var styleText = styleButton.GetComponentInChildren<TMPro.TMP_Text>();
                                styleText.text = door.name;
                            }
                            break;

                        case KitchenComponent.ComponentType.Drawers:
                            swapData.componentName.text = "Drawer Styles";

                            foreach (GameObject drawer in currentData.drawerOptions)
                            {
                                GameObject styleButton = PoolGenerator.instance.GetPooledObject("StyleSwatch");
                                styleButton.SetActive(true);
                                styleButton.transform.SetParent(swapData.optionArea.transform, false); // false was not there before - JAB
                                styleButton.tag = "Drawer";
                                var styleText = styleButton.GetComponentInChildren<TMPro.TMP_Text>();
                                styleText.text = drawer.name;
                            }
                            break;
                            
                        case KitchenComponent.ComponentType.Appliance:
                            swapData.componentName.text = "Appliance Styles";

                            foreach (GameObject appliance in currentData.applianceOptions)
                            {
                                GameObject styleButton = PoolGenerator.instance.GetPooledObject("StyleSwatch");
                                styleButton.SetActive(true);
                                styleButton.tag = "ApplianceOpt";
                                styleButton.transform.SetParent(swapData.optionArea.transform, false);
                                var styleText = styleButton.GetComponentInChildren<TMPro.TMP_Text>();
                                styleText.text = appliance.name;
                            }
                            break;

                    }
                }

                //Add row for enabling/disabling component visibility
                if (currentData.components[i].visibilityOption == KitchenComponent.VisibilityOption.yes)
                {
                    ////Add the toggle option
                    GameObject visibilityToggle = panelData.visibilityOption.gameObject;
                    visibilityToggle.SetActive(true);

                    //Check to see if the toggle should appear unchecked 
                    //because it was previously turned off
                    GameObject toggledObject = null;
                    bool isChecked = visibilityToggle.GetComponent<Toggle>().isOn;

                    if (currentData.components[i].identityType == KitchenComponent.AccessoryType.knob) {
                        toggledObject = currentData.knobs;
                        visibilityToggle.tag = "Knobs";
                    } else if (currentData.components[i].identityType == KitchenComponent.AccessoryType.pull)
                    {
                        toggledObject = currentData.pulls;
                        visibilityToggle.tag = "Pulls";
                    }

                    if (toggledObject.activeInHierarchy)
                    {
                        isChecked = true;
                    } else
                    {
                        isChecked = false;
                    }
                    
                }
            }
        }
    }

    //Controls the panel animation
    public void PanelViewer()
    {        
        if (!panelActive)
        {
            //anim.SetTrigger("slideIn");
            panelActive = true;
            panelToggle.Show();
        } else
        {
            //anim.SetTrigger("slideOut");
            panelActive = false;
            panelToggle.Hide();
                        
        }
    }

 
    private void Update()
    {
        //Makes sure the Worldpace UI icon always faces the camera
        transform.LookAt(Camera.main.transform);
    }



}
