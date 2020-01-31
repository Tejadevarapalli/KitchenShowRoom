using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    
    public bool buttonPressed;
    public bool buttonWasPressed;
    private bool buttonClickedThisFrame;
    private Button myButton;
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonWasPressed = buttonPressed;
        buttonPressed = true;
        myButton.onClick.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonWasPressed = buttonPressed;
        buttonPressed = false;        
    }

    private void Start()
    {
        myButton = GetComponent<Button>();        
    }

    private void Update()
    {
        if (buttonPressed && !buttonWasPressed)
            myButton.onClick.Invoke();        
    }
}