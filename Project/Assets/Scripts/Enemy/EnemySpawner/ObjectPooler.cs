using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //The pool needs to have 3 type of enemies (Low,Medium,High) being each one the type of tier of enemy (the higher the more powerful it is)
    [Serializable]
    public class Pool
    {
        public EnemyDifficulty enemyDiff;
        public GameObject prefab;
    }

    public Pool[] pools;
    List<Pool> optionalEnemies;

    public static ObjectPooler Instance { private set; get; }

    private void Awake()
    {
        Instance = this;

        optionalEnemies = new List<Pool>();
    }

    //called to spawn a object from the pool
    public void SpawnFromPool(EnemyDifficulty diff, Vector3 position, Quaternion rotation)
    {
        Debug.Log("this got here");
        if(pools == null)
        {
            Debug.Log("[ObjectPooler] There is no enemies in the pool");
            return;
        }

        foreach(Pool p in pools)
        {
            if(p.enemyDiff == diff)
            {
                optionalEnemies.Add(p);
            }
        }

        if (optionalEnemies != null)
        {
            int toSpawn = UnityEngine.Random.Range(0, optionalEnemies.Count);
            GameObject objectToSpawn = Instantiate(optionalEnemies[toSpawn].prefab, position, Quaternion.identity);

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();


            if (pooledObj != null)
            {
                pooledObj.OnObjectSpawn();
            }

            optionalEnemies.Clear();
        }

        return;
    }
}
public enum EnemyDifficulty { NORMAL, RARE, LEGENDARY}



/* before changes to spawner

    //The pool needs to have 3 type of enemies (Low,Medium,High) being each one the type of tier of enemy (the higher the more powerful it is)
    [Serializable]
    public class Pool
    {
        public EnemyDifficulty enemy;
        public GameObject prefab;
        public int size; //the size is the amount of objects that the pool will create at the start
    }

    public List<Pool> pools;
    public Dictionary<EnemyDifficulty, Queue<GameObject>> poolDictionary;

    public static ObjectPooler Instance { private set; get; }

    private void Awake()
    {
        Instance = this;

        poolDictionary = new Dictionary<EnemyDifficulty, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.enemy, objectPool);
        }
    }

    //called to spawn a object from the pool
    public GameObject SpawnFromPool(EnemyDifficulty diff, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(diff))
        {
            Debug.LogWarning("Pool with tag "+ tag + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[diff].Dequeue();

        objectToSpawn.SetActive(true);

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        
        poolDictionary[diff].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    */
