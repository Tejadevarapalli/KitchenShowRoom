using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFocus : MonoBehaviour
{
    [Tooltip("If enabled, the camera will automatically focus on an object selected for editing. This should be placed on the InfoShower button under Worldspace UI.")]
    public bool AutoFocusOnSelect;

    public GameObject optionsPanel;
    
    private CameraController camControl;

    private void Awake()
    {
        camControl = FindObjectOfType<CameraController>();        
    }

    public void Focus()
    {
        if (AutoFocusOnSelect && optionsPanel.activeInHierarchy)
        {
            if (camControl == null)
                camControl = FindObjectOfType<CameraController>();

            if (camControl == null)
                return;

            if (!camControl.NewObjectSelected)
                return;

            camControl.FocusOnObject();
        }
    }

}
