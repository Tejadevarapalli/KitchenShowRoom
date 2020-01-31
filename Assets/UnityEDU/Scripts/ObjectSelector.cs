using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DimBoxes;
using UnityEngine.Events;

     /* Controls the selection and deselection of objects in the scene. Only one object is allowed to be selected at
      * one time. If another object is clicked on, the previously selected object is set as deselected.*/
public class ObjectSelector : MonoBehaviour {
   
   [HideInInspector]
   public BoundBox selectionScript;

   public bool isSelected = false;

   private CameraController camScript;

    private void Awake()
    {
        selectionScript = GetComponent<BoundBox>();
        selectionScript.enabled = false;
        selectionScript.permanent = true;
        camScript = Camera.main.GetComponent<CameraController>();
        CameraController.UpdateSelection += CheckSelectionStatus;
        
    }

    public void CheckSelectionStatus()
    {
        
        if (camScript.selectedObj.gameObject == this.gameObject)
        {
            IsSelected = true;
        } else
        {
            IsSelected = false;
        }
    }

    public bool IsSelected
    {
        get { return isSelected; }
        set
        {
            if (value == isSelected)
                return;

            isSelected = value;
            if (isSelected)
            {
                selectionScript.enabled = true;
            } else
            {
                selectionScript.enabled = false;
            }
        }
    }
}
