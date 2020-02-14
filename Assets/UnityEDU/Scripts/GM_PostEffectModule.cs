//This script contains the Game Manager module that manages post processing image effects. It is responsible for playin the 
//graphical effect when the player takes damage

using UnityEngine;
using UnityEngine.PostProcessing;

public class GM_PostEffectModule : MonoBehaviour 
{
	[SerializeField] PostProcessingProfile defaultProfile;	//Default post processing profile
	[SerializeField] PostProcessingProfile damageProfile;	//Damaged post processing profile
	[SerializeField] float damageEffectDuration = 1f;		//Duration of the damage effect

	PostProcessingBehaviour postBevaiour;					//Reference to the HMD's post processing behaviour
	PostProcessingProfile profile;							//A profile used for calculations during the game
	Camera mainCamera;										//The HMD camera
	bool isDamaged;											//Is the player currently damaged?
	float elapsedTime;										//The amount of time that hand passed
	bool vignetteEnabled;									//Was vignetting enabled in the default profile?
	bool aberrationEnabled;									//Was chromatic aberration enabled in the default profile?


	void Start () 
	{
		//Get a reference to the HMD camera and the post processing behaviour component attached to it
		mainCamera = GameManager.instance.HMD;
		postBevaiour = mainCamera.GetComponent<PostProcessingBehaviour> ();

		//Instantiate a new profile based on the default one. This is done to prevent the original
		//profile from being changed during gameplay
		profile = Object.Instantiate (defaultProfile) as PostProcessingProfile;
		postBevaiour.profile = profile;
	}

	void Update()
	{
		//If the player is currently being damaged, process the effect
		if (isDamaged)
			ProcessDamageEffect ();
	}

	//This method is called when the player takes damage and starts the visual effect process
	public void PlayDamageEffect()
	{
		//If the player is not currently damaged...
		if (!isDamaged)
		{
			//...record whether vignetting and chromatic aberration are enabled by default
			vignetteEnabled = profile.vignette.enabled;
			aberrationEnabled = profile.chromaticAberration.enabled;
		}

		//Enable vignetting and set the amount equal to the damaged profile
		profile.vignette.enabled = true;
		profile.vignette.settings = damageProfile.vignette.settings;

		//Enable chromatic aberration and set the amount equal to the damaged profile
		profile.chromaticAberration.enabled = true;
		profile.chromaticAberration.settings = damageProfile.chromaticAberration.settings;

		//Reset the ellapsed time and set isDamaged to true
		elapsedTime = 0f;
		isDamaged = true;
	}

	//This method will be called every frame while the damage visual effect is active
	void ProcessDamageEffect()
	{
		//Record the ellapsed time and calculate the percentage of the effect that is complete
		elapsedTime += Time.deltaTime;
		float percentage = elapsedTime / damageEffectDuration;

		//Grab the current vignette settings...
		VignetteModel.Settings vignette = profile.vignette.settings;
		//...Lerp the color value...
		vignette.color = Color.Lerp (damageProfile.vignette.settings.color, Color.white, percentage);
		//...and then apply the Lerped changes
		profile.vignette.settings = vignette;

		//Grab the current aberration settings...
		ChromaticAberrationModel.Settings chroma = profile.chromaticAberration.settings;
		//...Lerp the intensity...
		chroma.intensity = Mathf.Lerp (damageProfile.chromaticAberration.settings.intensity, 0f, percentage);
		//... and then apply the Lerped changes
		profile.chromaticAberration.settings = chroma;

		//If the time has reached 100% stop the effect
		if (percentage >= 1f)
			StopDamageEffect ();
	}

	void StopDamageEffect()
	{
		//Set the vignette and chromatic aberration settings back to default
		profile.chromaticAberration.enabled = aberrationEnabled;
		profile.vignette.enabled = vignetteEnabled;
		//The damaged effect is no longer playing
		isDamaged = false;
	}
}
