//This scriptable object represents the dataset for a single enemy wave

using UnityEngine;

//This attribute will allow us to create EnemyWave assets in the editor
[CreateAssetMenu(fileName = "EnemyWave", menuName = "Enemy Spawning/Enemy Wave", order = 1)]
public class EnemyWave : ScriptableObject 
{
	public int numberOfRangedEnemies = 1;	//Number of ranged enemies in a wave
	public int numberOfMeleeEnemies = 1;	//Number of melee enemies in a wave
}