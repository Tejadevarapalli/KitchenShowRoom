//This script manages the ranged drones movement and shooting behaviours

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangedDrone : MonoBehaviour 
{
	const string projectilePoolName = "Bullet";			//The name of the bullet prefab in the Object Pool
	const string explosionPoolName = "Explosion";		//The name of the explosion prefab in the Object Pool

	[SerializeField] int pointValue = 250;				//The amount of points this enemy is worth
	[SerializeField] Transform droneBody;				//A reference to the drone's head. Used for turning to face the player

	[Header("Movement Properties")]
	[SerializeField] float navRadius = 9f;				//The maximum radius the drone flies around the player
	[SerializeField] float minHeight = 3f;				//The minimum flying height for the drone
	[SerializeField] float maxHeight = 9f;				//The maximum flying height for the drone
	[SerializeField] float heightChangeDampen = .5f;	//A dampener that slows the rate that the drone changes height

	[Header("Shooting Properties")]
	[SerializeField] VFXObject chargeEffectsObject;		//A reference to the VFX object that shows the drone charging up a shot
	[SerializeField] Transform firePoint;				//The point that the bullets will fire from
	[SerializeField] float minFireRate = 3f;			//Minimum rate of firing
	[SerializeField] float maxFireRate = 6f;			//Maximum rate of firing
	[SerializeField] float chargeDuration = 1.2f;		//The duration of the bullet charge VFX

	Transform target;									//A reference to the drone's target
	bool isAlive = true;								//Is the drone currently alive?
	float desiredHeight;								//The height that the drone is trying to reach
	NavMeshAgent agent;									//A reference to the navmesh agent component
	float cooldown;										//The cooldown between shot

	WaitForSeconds shotDelay;							//The delay between shots
	WaitForSeconds chargeDelay;							//The delay while waiting for a shot to charge


	//The Reset() method will attempt to grab values in the editor to save time in finding references
	void Reset()
	{
		//Find the Charge Effects object, Fire Point object, and Drone Body object
		chargeEffectsObject = GetComponentInChildren<VFXObject> ();
		firePoint = transform.Find ("Drone_Ranged_Body/FirePoint");
		droneBody = transform.Find ("Drone_Ranged_Body");
	}

	void Start()
	{
		//Get a reference to the player as a target
		target = GameManager.instance.player;
		//Get a refernce to the navmesh agent component
		agent = GetComponent<NavMeshAgent> ();

		//Initialize the charge delay and start the movement cycle
		chargeDelay = new WaitForSeconds (chargeDuration);
		StartCoroutine (MovementCycle ());
	}

	void Update()
	{
		//Turn the drone's head to face the player
		droneBody.LookAt(target.position);
	}

	IEnumerator MovementCycle()
	{
		//Determine a random cooldown for this drone and set the ellapsed time to 0
		cooldown = Random.Range (minFireRate, maxFireRate);
		float ellapsedTime = 0f;

		while (isAlive)
		{
			//If the navmesh agent is active and has reached its destination OR it is stopped...
			if (agent.isActiveAndEnabled && (agent.remainingDistance <= agent.stoppingDistance || agent.isStopped))
			{
				//...determine if it is time to shoot. If it is...
				if (ellapsedTime >= cooldown)
				{
					//...start the shooting cycle and wait for it to finish...
					yield return StartCoroutine (ShootingCycle ());
					//...then reset the ellapsed time and pick a new random cooldown
					ellapsedTime = 0f;
					cooldown = Random.Range (minFireRate, maxFireRate);
				}
				//...if it isn't time to shoot, then pick a new destination
				SetNewDestination ();
			}

			//We use the baseOffset of the navmesh agent to make the drone's body move up and down in the game. We use a Lerp to smoothly
			//move from the current height to the desired height
			agent.baseOffset = Mathf.Lerp (agent.baseOffset, desiredHeight, Time.deltaTime * heightChangeDampen);

			//Record the new time and then wait for the next frame to loop back around
			ellapsedTime += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator ShootingCycle()
	{
		//Start the shooting cycle by playing the charging shot effect
		chargeEffectsObject.Play ();

		//Wait for the effect to finish
		yield return chargeDelay;

		//Get a bullet from the Object Pool
		GameObject obj = ObjectPoolManager.instance.GetObject(projectilePoolName, true, firePoint.position, firePoint.rotation);

		//If we didn't get a bullet, an error has occured and we should exit this shooting cycle
		if (obj == null)
			yield break;

		//Turn the bullet to look at the player
		obj.transform.LookAt (target.position);

		//Look for the Bullet script on the bullet and tell it that this drone was its source
		Bullet bullet = obj.GetComponent<Bullet> ();
		if (bullet != null)
			bullet.source = transform;
	}

	void SetNewDestination()
	{
		//Calculate a new random height
		desiredHeight = Random.Range (minHeight, maxHeight);

		//Find a random point in a large circle around the player
		Vector2 v = Random.insideUnitCircle * navRadius;
		//Modify the random position to ignore the Y value and to prevent negative Z values so that the 
		//drone can never go behind the player
		Vector3 correctedVector = new Vector3 (v.x, 0f, Mathf.Abs (v.y));

		//Use a NavmeshHit variable to make sure that the random point determined above actually sits on the
		//navmesh. If it doesn't, find the closest spot that does
		NavMeshHit hit;
		NavMesh.SamplePosition (correctedVector, out hit, navRadius, 1);

		//Tell the agent to go to the spot
		agent.SetDestination(hit.position);
	}

	//This method will be called if the drone needed to be stopped immediately
	public void Die()
	{
		//Set the drone to be not alive
		isAlive = false;

		//Stop any charging effects
		chargeEffectsObject.Stop ();

		//Stop all coroutines (movement and shooting cycles)
		StopAllCoroutines ();
	}

	void OnCollisionEnter(Collision other)
	{
		//If the drone has collided with anything other than a bullet, exit
		if (!other.gameObject.CompareTag ("Bullet"))
			return;

		//Get an explosion prefab from the Object Pool
		ObjectPoolManager.instance.GetObject (explosionPoolName, true, transform.position);

		//Tell the Game Manager to add more points
		GameManager.instance.AddPoints(pointValue);

		//Destroy this game object
		Destroy(gameObject);
	}
}
