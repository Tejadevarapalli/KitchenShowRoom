using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToggleVisibility : MonoBehaviour
{

    public GameObject target;
    [Tooltip("(Optional) The alternate target will always be invisible when the primary target is visible, and vice versa.")]
    public GameObject altTarget;
    [Tooltip("This text displays when the primary target is visible.")]
    public string textWhenVisible = "Hide\nUI";
    [Tooltip("This text displays when the primary target is invisible.")]
    public string textWhenInvisible = "Show\nUI";
    [Tooltip("Should the script set all children of the target(s) active/inactive, or just the target(s)?")]
    public bool recursive;

    bool targetActive;
    private TMP_Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        targetActive = target.activeSelf;
        buttonText = gameObject.GetComponentInChildren<TMPro.TMP_Text>();
        Set();        
    }

    public void ToggleVis()
    {
        targetActive = !targetActive;
        Set();
    }

    public void Hide()
    {
        targetActive = false;
        Set();
    }

    public void Show()
    {
        targetActive = true;        
        Set();
    }

    private void Set()
    {
        if (recursive)
            target.SetActiveRecursively(targetActive);
        else
            target.SetActive(targetActive);
        buttonText.text = targetActive ? textWhenVisible : textWhenInvisible;
        if (altTarget != null)
        {
            if (recursive)
                altTarget.SetActiveRecursively(!targetActive);
            else
                altTarget.SetActive(!targetActive);
        }
            
    }
}
