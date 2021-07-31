using UnityEngine;


[CreateAssetMenu(fileName = "New Wave")]
public class EnemySpawnerSO : ScriptableObject
{
    [HideInInspector]public int enemyCount;
    //initialized in unity
    public float spawnDelay;
    public int maxEnemies;
}
