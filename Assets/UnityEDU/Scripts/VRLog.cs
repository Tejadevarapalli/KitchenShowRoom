//This script manages the VR Log which is a worldspace UI tht acts as a console so that players
//can see diagnostic output without taking their VR HMD off

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class VRLog : MonoBehaviour 
{
	public static VRLog instance;				//Singleton reference to the VRLog game object

	[SerializeField] Text logText;				//The text UI object
	[SerializeField] bool showInEditor = true;	//Should the VRLog be visible in the editor?
	[SerializeField] bool showInBuild = false;	//Should the VRLog be visible in a build?
	[SerializeField] bool isVisible = true;		//Is the VRLog currently visible?

	Queue<string> items;						//The collection of text items
	StringBuilder builder;						//We use a string builder to efficiently build our log messages
	ScrollRect scroll;							//The scroll rect that contains the VRLog
	CanvasGroup canvasGroup;					//A reference to the canvas group that will handle showing and hiding the log

	const int MAXITEMS = 100;					//The total number of log items we can have
	float scrollVertPosition = 0f;				//The current position of the scroll rect


	void Awake()
	{
		//If we either don't want to see the VRLog or this isn't the first VRLog, destroy this game object and then exit
		if((!showInEditor && Application.isEditor) ||
			(!showInBuild && !Application.isEditor) ||
			(instance != null && instance != this))
		{
			Destroy(gameObject);
			return;
		}

		instance = this;

		//Initialize the text queue and string builder
		items = new Queue<string> (MAXITEMS);
		builder = new StringBuilder ();

		//Get references to the scroll rect and canvas group components
		scroll = GetComponentInChildren<ScrollRect> ();
		canvasGroup = GetComponent<CanvasGroup> ();

		//Either show or hide the log
		ToggleVisible (isVisible);
	}

	void Update()
	{
		//If the player presses the BackQuote key (`), toggle the log
		if (Input.GetKeyDown (KeyCode.BackQuote))
			ToggleVisible (!isVisible);		
	}

	//This method is called throughout the code when  message should be logged
	public static void Log(string message)
	{
		//If there is no VRLog, print the message to the console. Otherwise, log the message
		if (instance == null)
			Debug.Log (message);
		else
			instance.InternalLog (message);
	}

	protected void InternalLog(string message)
	{
		//Make sure the Queue has enough room for more messages
		CheckCapacity ();

		//Enqueue a new message and append the message to our text string
		items.Enqueue (message);
		builder.Append (message);
		builder.Append ("\n");

		//Write the full log out to the UI
		logText.text = builder.ToString ();
	}

	void CheckCapacity()
	{
		//If the queue has enough room, exit
		if (items.Count < MAXITEMS - 1)
			return;

		//Otherwise, remove the oldest message from the queue and the string builder
		string removedItem = items.Dequeue ();
		builder.Remove (0, removedItem.Length + 1);
	}

	public void MoveScrollRect(float amount)
	{
		//Move the scroll rect up or down
		scrollVertPosition = Mathf.Clamp (scrollVertPosition + amount, 0f, 1f);
		scroll.verticalNormalizedPosition = scrollVertPosition;
	}

	void ToggleVisible(bool value)
	{
		if (value == isVisible)
			return;

		//Toggle the visibility of the UI by changing the alpha of the canvas group
		isVisible = value;
		canvasGroup.alpha = isVisible ? 1f : 0f;
	}
}
