//This script contains the Game Manager module that manages waves of enemy spawning. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_SpawnModule : MonoBehaviour 
{
	[SerializeField] bool isEnabled = true;				//Should enemies spawn?
	[SerializeField] float checkRate = .5f;				//The amount of time in between checks to determine if a wave is over
	[SerializeField] EnemyWave[] waves;					//The collection of enemy waves

	[Header("Ranged Drones")]
	[SerializeField] GameObject rangedEnemy;			//The ranged drone prefab
	[SerializeField] Transform[] rangedSpawnPoints;		//The collection of spawning points for ranged drones

	[Header("Melee Drones")]
	[SerializeField] GameObject meleeEnemy;				//The melee drone prefab
	[SerializeField] Transform[] meleeSpawnPoints;		//The collection of spawning points for melee drones

	List<MeleeDrone> melee;								//The collection of melee drones in a wave
	List<RangedDrone> ranged;							//The collection of ranged drones in a wave
	WaitForSeconds checkDelay;							//Delay container
	int currentWave;									//The number of the current wave
	int totalWavesSpawned = 0;							//The total number of waves spawned this playthrough

	void Start()
	{
		//Create the lists to contain the enemies
		melee = new List<MeleeDrone> ();
		ranged = new List<RangedDrone> ();
		//Initialize the delay between checks
		checkDelay = new WaitForSeconds (checkRate);
		//Start the spawning cycle
		StartCoroutine (SpawnCycle ());
	}

	IEnumerator SpawnCycle()
	{
		//Initially wait before spawning to give the player time to prepare
		yield return checkDelay;

		while (isEnabled)
		{
			//Spawn a new wave
			SpawnNewWave ();

			//After spawning a wave, wait for the wave to be defeated. Instead of checking every frame, a check occurs
			//at a slower interval to reduce CPU overhead. The effect is more efficient and not noticeable to the player
			do
			{
				//Delay in between checks
				yield return checkDelay;
			}
			while(!CheckForEndOfWave ());

			//Once the wave is over, increment the current wave and then loop back around to spawn a new one. If we've reached
			//the end of the waves array, start back at 0
			if (++currentWave >= waves.Length)
				currentWave = 0;
		}
	}

	void SpawnNewWave()
	{
		//Tell the Game Manager how many waves have spawned so it can update the UI
		GameManager.instance.NewWaveSpawned (++totalWavesSpawned);

		//Clear out our lists
		ranged.Clear ();
		melee.Clear ();

		//Iterate through the number of ranged enemies in this wave
		for (int i = 0; i < waves [currentWave].numberOfRangedEnemies; i++)
		{
			//Pick a random spawn point
			int index = Random.Range (0, rangedSpawnPoints.Length);
			//Instantiate the enemy
			GameObject obj = Instantiate (rangedEnemy) as GameObject;
			//Place the enemy at the spawn point
			obj.transform.position = rangedSpawnPoints [index].position;
			//Add the enemy to the collection
			ranged.Add (obj.GetComponent<RangedDrone> ());
		}

		//Iterate through the number of melee enemies in this wave
		for (int i = 0; i < waves [currentWave].numberOfMeleeEnemies; i++)
		{
			//Pick a random spawn point
			int index = Random.Range (0, meleeSpawnPoints.Length);
			//Instantiate the enemy
			GameObject obj = Instantiate (meleeEnemy) as GameObject;
			//Place the enemy at the spawn point
			obj.transform.position = meleeSpawnPoints [index].position;
			//Add the enemy to the collection
			melee.Add (obj.GetComponent<MeleeDrone> ());
		}
	}

	//This method is called to determine if a wave has been defeated. A value of false will be returned if the wave
	//is still active, while true means that it is complete
	bool CheckForEndOfWave()
	{
		//Loop through the ranged enemies, and if a living one is found, return false
		for (int i = 0; i < ranged.Count; i++)
			if (ranged [i] != null)
				return false;

		//Loop through the melee enemies, and if a living one is found, return false
		for (int i = 0; i < melee.Count; i++)
			if (melee [i] != null)
				return false;

		//If no ranged or melee enemies were found, then the wave is over and return true
		return true;
	}

	public void Stop()
	{
		//Stops the spawning cycle
		isEnabled = false;
	}
}
