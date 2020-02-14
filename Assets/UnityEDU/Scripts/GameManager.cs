//This script maintains references to all of the important parts of the game so that references don't need
//to be found at runtime. The script also maintains the game information as well as exiting functionality

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;			//Singleton instance reference

	public Transform player;					//Reference to the player's body target
	public Camera HMD;							//Reference to the HMD camera
	public GM_SpawnModule enemySpawner;			//Reference to the enemy spawner
	public GM_HandModule hands;					//Reference to the module that manages the player's hands
	public GM_PostEffectModule postEffects;		//Reference to the module that manages the post processing effects

	[Header("UI Properties")]
	[SerializeField] Text scoreText;
	[SerializeField] Text waveText;
	[SerializeField] Text livesText;

	int score = 0;


	void Awake()
	{
		//Make sure we only have one game manager. If this is the first Game Manager, we initialize our references.
		if (instance == null)
		{
			instance = this;
			InitializeReferences ();
		}
		//If this is not the first Game Manager then we destroy this Game Manager
		else if (instance != this)
		{
			Destroy (gameObject);
		}
	}

	void Start()
	{
		//Initialize the score UI
		scoreText.text = score.ToString ();
	}

	//This method gets the reference to the various components if they don't already exist
	void InitializeReferences()
	{
		if (enemySpawner == null)
			enemySpawner = GetComponent<GM_SpawnModule> ();
		
		if(hands == null)
			hands = GetComponent<GM_HandModule> ();

		if(postEffects == null)
			postEffects = GetComponent<GM_PostEffectModule> ();
	}

	public void SetPlayerLives(int lives)
	{
		livesText.text = lives.ToString ();
	}

	public void NewWaveSpawned(int wave)
	{
		waveText.text = wave.ToString ();
	}

	public void AddPoints(int scoreValue)
	{
		score += scoreValue;

		scoreText.text = score.ToString ();
	}
}
