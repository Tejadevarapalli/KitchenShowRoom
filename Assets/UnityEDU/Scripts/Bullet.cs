//This script manages the bullet objects. It handles the motion of the bullet as well as calculating 
//deflections (bounces) from the player's swords

using UnityEngine;

public class Bullet : MonoBehaviour 
{
	const string impactPoolName = "Impact";			//The name of the impact prefabs that are pooled
	const string bulletLayerName = "Bullet";		//The name of the layer the bullet starts on
	const string bounceLayerName = "BounceBullet";	//The name of the layer the bullet will be on after a bounce

	public Transform source;						//Reference to the drone that fired this bullet
	[SerializeField] float timeToLive = 5f;			//The life time of the bullet
	[SerializeField] float normalSpeed = 5f;		//The speed that the bullet travels
	[SerializeField] float bounceSpeed = 10f;		//The speed that the bullet travels after a bounce

	[Header("Deflection Properties")]
	[SerializeField] float perfectSwordSpeed = 20f;	//Speed of the sword in order to get a tracking bounce
	[SerializeField] float minSwordSpeed = 7f;		//Minimum sword speed needed for a straight bounce
	[SerializeField] float perfectSwordAngle = 30f;	//Angle of collision required for a tracking bounce 
	[SerializeField] float minSwordAngle = 90f;		//Minimum collision angle needed for a straight bounce

	Rigidbody rigidBody;							//Reference to the rigidbody
	Bullet_Audio projectileAudio;					//Reference to the Bullet Audio script

	float currentSpeed;								//Current speed of the bullet
	bool isPerfectBounce;							//Was the bullet deflected "perfectly"?

	void Start()
	{
		//Get references needed in the script
		rigidBody = GetComponent<Rigidbody> ();
		projectileAudio = GetComponent<Bullet_Audio> ();
	}
		
	//Will be called every time a bullet is taken out of the object pool. Since bullets will be 
	//reused, initialization is handled here
	void OnEnable()
	{
		//Set the current speed
		currentSpeed = normalSpeed;
		//Place the bullet on the initial layer
		gameObject.layer = LayerMask.NameToLayer (bulletLayerName);
		//So far, this is not a perfect bounce
		isPerfectBounce = false;
		//Cancel any Invoke commands currently running
		CancelInvoke ();
		//Invoke the Disable method after the required amount of time
		Invoke ("Disable", timeToLive);
	}

	void FixedUpdate()
	{
		//Calculate the position the bullet should have after this frame and then move the rigidbody to that position
		Vector3 newPos = transform.position + transform.forward * currentSpeed * Time.deltaTime;
		rigidBody.MovePosition (newPos);

		//If the bullet has been deflected "perfectly", make the bullet track back to its source like a seeking missile
		if (isPerfectBounce)
			rigidBody.MoveRotation (Quaternion.LookRotation (source.position - transform.position));
	}

	void OnCollisionEnter(Collision other)
	{
		//Try to get a Sword script off of the object collided with
		Sword sword = other.gameObject.GetComponent<Sword> ();
		//If the object doesn't have a sword script...
		if (sword == null)
		{
			//...Disable this bullet and then exit
			Disable ();
			return;
		}

		//Get the point of contact
		ContactPoint contact = other.contacts [0];
		//Use the point of contact to calculate the bounce for the bullet
		CalculateBounce (sword,  contact);
		//Now that the bullet has bounced set it to the appropriate layer
		gameObject.layer = LayerMask.NameToLayer(bounceLayerName);
		//Get an impact effect from the object pool at the point of collision
		ObjectPoolManager.instance.GetObject (impactPoolName, true, contact.point);
	}

	//This method calculates the angle and speed of the collision to determine if it is a normal bounce,
	//straight bounce, or perfect bounce
	void CalculateBounce(Sword sword, ContactPoint contact)
	{
		//Get the vector of reflection
		Vector3 bounceVector = Vector3.Reflect (transform.forward, contact.normal);
		//Use the reflection vector to determine the angle of collision
		float angle = Vector3.Angle (transform.forward * -1, bounceVector);

		//This is a perfect bounce if the sword is going faster than the required speed OR has an angle smaller than the required angle
		isPerfectBounce = sword.Speed > perfectSwordSpeed || angle < perfectSwordAngle;

		//If this is a perfect bounce OR the speed and angle are above the minimum required...
		if (isPerfectBounce || (sword.Speed > minSwordSpeed && angle < minSwordAngle))
		{
			//...turn the bullet to face its source (straight bonuce)...
			transform.LookAt (source.position);
			//...set its current speed to the bounce speed..
			currentSpeed = bounceSpeed;

			//...and if there is a projectile audio script attached, tell it to play the appropriate sound
			if (projectileAudio != null) 
				projectileAudio.PlayStraightDeflection (isPerfectBounce);

			//Finally, log the collision type, speed, and angle
			VRLog.Log ("Straight Deflection (" + isPerfectBounce + "): " + sword.Speed + " / " + angle);
		}
		//Otherwise, this is a normal deflection type bounce (physically accurate bounce)
		else
		{
			//Rotate the bullet to face the bounce direction...
			transform.rotation = Quaternion.LookRotation (contact.normal);

			//...and if there is a projectile audio script attached, tell it to play the appropriate sound
			if (projectileAudio != null) 
				projectileAudio.PlayBounceDeflection ();

			//Finally, log the collision type, speed, and angle
			VRLog.Log ("Bounce Deflection: " + sword.Speed + " / " + angle);
		}
	}

	void Disable()
	{
		//Disable the bullet so it can be reused by the Object Pool
		gameObject.SetActive (false);
	}
}
