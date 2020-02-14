//This script manages the player's health and is responsible for telling the post processing module when to play the 
//damaged effects

using UnityEngine;
using UnityEngine.Events;

//This script requires an AudioSource to function
[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour 
{
	[SerializeField] int lives = 5;				//The number of lives the player starts with
	[SerializeField] AudioClip[] soundClips;	//An array of different hurt sound effects
	[SerializeField] UnityEvent onDamaged;		//A UnityEvent that will be called when the player is hurt. This can be configured through the editor
	[SerializeField] UnityEvent onKilled;		//A UnityEvent that will be called when the player is killed. This can be configured through the editor

	AudioSource audioSource;					//A reference to the audio source
	int currentLives;							//The amount of lives the player currently has


	void Start()
	{
		//Set the current number of lives
		currentLives = lives;

		//Get a reference to the audio source and configure it
		audioSource = GetComponent<AudioSource> ();
		audioSource.playOnAwake = false;
		//Set the audio source to play 3D audio
		audioSource.spatialBlend = 1;	

		//Tell the Game Manager how many lives the player has
		GameManager.instance.SetPlayerLives(currentLives);
	}

    void OnCollisionEnter(Collision other)
    {
        //If the player is already dead, exit
        if (currentLives <= 0)
        {
            return;
        }


        //If the player didn't collide with a melee enemy or a bullet, exit
        if (!other.gameObject.CompareTag ("MeleeEnemy") && !other.gameObject.CompareTag ("Bullet"))
			return;

		//If we have at least 1 audio clip
		if (soundClips.Length > 0)
		{
			//Pick a random audio clip and play it as a One Shot. Allows the audio source to play
			//multiple clips at the same time if need be
			int soundIndex = Random.Range (0, soundClips.Length);
				audioSource.PlayOneShot (soundClips [soundIndex]);
		}

		//Invoke the OnDamaged event
		onDamaged.Invoke ();

        //If the player is now out of lives, invoke the OnKilled event
        if (--currentLives <= 0)
        {
			onKilled.Invoke ();

        }

		//Tell the Game Manager how many lives the player has
		GameManager.instance.SetPlayerLives(currentLives);
	}
}
