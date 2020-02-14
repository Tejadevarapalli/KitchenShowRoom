//This script manages the melee drone. It is responsible for moving the drone towards the player and destroying the drone when it collides with the player

using System.Collections;
using UnityEngine;
using NobleMuffins.TurboSlicer;
using UnityEngine.AI;

public class MeleeDrone : MonoBehaviour 
{
	const string explosionPoolName = "Explosion";	//The name of the explosion prefab in the Object Pool	

	[SerializeField] int pointValue = 100;			//The amount of points this enemy is worth
	[SerializeField] float minTargetHeight = 1f;	//Minimum height the drone will reach
	[SerializeField] float maxTargetHeight = 2.5f;	//Maximum height the drone will reach
	[SerializeField] float minSpeed = 1f;			//Minimum speed the drone will fly
	[SerializeField] float maxSpeed = 2.5f;			//Maximum speed the drone will fly

	Transform target;								//The drone's target
    NavMeshAgent agent;                             //The drone's navmesh agent                             

	void Start()
	{
		//Set the target to the player
		target = GameManager.instance.player;
        
        //Set references to the NavmeshAgent
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(minSpeed, maxSpeed); 
        agent.baseOffset = Random.Range(minTargetHeight, maxTargetHeight);
    }

    private void FixedUpdate()
    {
        //Always look at the player as they move around the scene
        transform.LookAt(target.position);

        //Navigate to the player
        agent.SetDestination(target.position);
    }


    void OnCollisionEnter(Collision other)
    {
        //If the drone has collided with anything other than the player, exit this method
        if (!other.gameObject.CompareTag("Player"))
            return;

        //The drone has collided with the player. Use the Turber Slicer API to shatter it
        TurboSlicerSingleton.Instance.Shatter(gameObject, 1);

        //Get an explosion effect from the object pool
        ObjectPoolManager.instance.GetObject(explosionPoolName, true, transform.position);
    }

    //This method will be called when the drone is sliced by the Player's sword
    public void Sliced()
    {
        //Tell the Game Manager to add more points
        GameManager.instance.AddPoints(pointValue);
    }
}
