//This script contains the Game Manager module that manages the player's hand objects. It is responsible for switching
//between swords and VR Controllers

using UnityEngine;

//A simple Enum to contain the possible hand modes
public enum HandMode{Controllers, Swords};

public class GM_HandModule : MonoBehaviour 
{
	[SerializeField] HandMode startingMode;			//The starting hand mode

	[Header("Hand Objects")]
	[SerializeField] GameObject leftController;		//A reference to the left controller
	[SerializeField] GameObject leftSword;			//A reference to the left sword
	[SerializeField] GameObject rightController;	//A reference to the right controller
	[SerializeField] GameObject rightSword;			//A reference to the right sword


	void Start()
	{
		//Determine if the player starts with swords or controllers
		bool isSwords = startingMode == HandMode.Swords ? true : false;

		//Toggle the hands to have the correct object
		ToggleControllers (!isSwords);
		ToggleSwords (isSwords);
		
	}

	//This method turns the controllers on and off
	public void ToggleControllers(bool isVisible)
	{
		if(leftController != null) leftController.SetActive (isVisible);
		if (rightController != null) rightController.SetActive (isVisible);
	}

	//This method turns the swords on and off
	public void ToggleSwords(bool isVisible)
	{
		if(leftSword != null) leftSword.SetActive (isVisible);
		if (rightSword != null) rightSword.SetActive (isVisible);
	}

	//Public method to switch to the controller objects
	public void SwitchToControllers()
	{
		ToggleControllers (true);
		ToggleSwords (false);
	}

	//Public method to switch to the sword objects
	public void SwitchToSwords()
	{
		ToggleControllers (false);
		ToggleSwords (true);
	}
}
