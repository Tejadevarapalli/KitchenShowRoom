//This script manages VFX objects. It is required to ensure that audio and particle effects are started and
//stopped simulataneously

using UnityEngine;

public class VFXObject : MonoBehaviour 
{
	[SerializeField] AudioSource sounds;		//Reference to the audio source
	[SerializeField] ParticleSystem particles;	//Reference to the particle system

	void Reset()
	{
		//Get the audio source and particle system components
		sounds = GetComponent<AudioSource> ();
		particles = GetComponent<ParticleSystem> ();

		//If the components cannot be found on this game object, look in the children objects
		if (sounds == null)
			sounds = GetComponentInChildren<AudioSource> ();

		if (particles == null)
			particles = GetComponentInChildren<ParticleSystem> ();
	}

	public void Play()
	{
		//Stop all current plays of the VFX
		Stop ();

		//Play the audio and particles
		sounds.Play ();
		particles.Play (true);
	}

	public void Stop()
	{
		//Stop the audio if it is playing
		if(sounds.isPlaying)
			sounds.Stop ();

		//Stop the particle system if it is playing
		if(particles.isPlaying)
			particles.Stop (true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}
}
