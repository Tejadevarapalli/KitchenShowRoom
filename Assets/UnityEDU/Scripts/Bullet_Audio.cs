//This script handles playing the audio when a bullet is deflected by a sword. The script is necessary since
//different types of bounces should have different and distinct audio cues

using UnityEngine;

public class Bullet_Audio : MonoBehaviour 
{
	[SerializeField] float minVolume = .5f;	//The minimum volume
	[SerializeField] float maxVolume = 1f;	//The maximum volume
	[SerializeField] float minPitch = 1f;	//The minimum pitch
	[SerializeField] float maxPitch = 1.5f;	//The maximum pitch

	AudioSource audioSource;				//A reference to the audio source component

	void Start()
	{
		//Get the audio source reference
		audioSource = GetComponent<AudioSource> ();
	}

	//This method handles straight and "perfect" bounces
	public void PlayStraightDeflection(bool isPerfect)
	{
		//If this is a perfect bounce then use max volume. Otherwise, use the minimum
		audioSource.volume = isPerfect ? maxVolume : minVolume;
		//Set the audio source to use the maximum pitch
		audioSource.pitch = maxPitch;
		//Play the audio
		audioSource.Play ();
	}

	//This method handles normal bounces
	public void PlayBounceDeflection()
	{
		//Set the audio source to the minumim volume and pitch
		audioSource.volume = minVolume;
		audioSource.pitch = minPitch;
		//Play the audio
		audioSource.Play ();
	}
}
