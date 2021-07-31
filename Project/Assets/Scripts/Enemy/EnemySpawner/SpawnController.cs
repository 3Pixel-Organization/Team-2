using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class SpawnController : MonoBehaviour
{
    private ObjectPooler _objPooler;
    public static SpawnController Instance { private set; get; }

    public EnemySpawnerSO[] waveList;
    public GameObject[] spawnPoints;

    private int actualWave = 0;
    [HideInInspector] public int actualEnemiesAlive = 0;

    UIGameplayManager uiGameplay => UIGameplayManager.Instance;

    void Start()
    {
        Instance = this;

        _objPooler = ObjectPooler.Instance;
        foreach (EnemySpawnerSO e in waveList)
        {
            e.enemyCount = 0;
        }

        StartCoroutine(StartWave());

        foreach (GameObject o in spawnPoints)
        {
            o.GetComponent<Renderer>().enabled = false;
        }
    }

    // Choose which enemy to spawn from the object pool
    private int difficultyRate = 0;
    IEnumerator StartWave()
    {
        if(uiGameplay == null) yield return new WaitForSeconds(1);
        uiGameplay.waveState.gameObject.SetActive(true);
        uiGameplay.waveState.text = $"Wave {actualWave + 1} starting";
        yield return new WaitForSeconds(2f);
        uiGameplay.waveState.gameObject.SetActive(false);

        EnemySpawnerSO wave = waveList[actualWave];
        while (wave.enemyCount < wave.maxEnemies)
        {

            Vector3 spawnpoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            int randomNumber = Random.Range(0, 21);
            randomNumber += difficultyRate;

            if (randomNumber <= 13)
            {
                _objPooler.SpawnFromPool(EnemyDifficulty.NORMAL, spawnpoint, Quaternion.identity);
            }
            else if (randomNumber >= 14 && randomNumber <= 18)
            {
                _objPooler.SpawnFromPool(EnemyDifficulty.RARE, spawnpoint, Quaternion.identity);
            }
            else
            {
                _objPooler.SpawnFromPool(EnemyDifficulty.LEGENDARY, spawnpoint, Quaternion.identity);
            }

            yield return new WaitForSeconds(wave.spawnDelay);
            wave.enemyCount += 1;
            actualEnemiesAlive++;
        }
        StartCoroutine(WaitForEndofWave());
    }

    //prepare next wave and each 3 waves increase the probability to spawn higher tier enemies
    IEnumerator WaitForEndofWave()
    {
        while (actualEnemiesAlive > 0)
        {
            yield return new WaitForSeconds(1f);
        }

        uiGameplay.waveState.enabled = enabled;
        uiGameplay.waveState.text = "Wave finished";
        actualWave++;

        if (actualWave < waveList.Length)
        {
            StartCoroutine(StartingWave());
            yield return new WaitForSeconds(6f);
            if (actualWave % 3 == 0)
            {
                difficultyRate++;
            }

            uiGameplay.waveState.text = $"Wave {actualWave + 1}";
            yield return new WaitForSeconds(2);
            uiGameplay.waveState.enabled = false;
            StartCoroutine(StartWave());
        }
        else
        {
            uiGameplay.waveState.text = "Stage finished!";
            uiGameplay.waveState.enabled = true;
            yield return new WaitForSeconds(2);
            GameManager.Instance.GoToRestartScene();
        }
    }

    IEnumerator StartingWave()
    {
        uiGameplay.waitingWaveStartText.gameObject.SetActive(true);
        for (int i = 5; i >= 0; i--)
        {
            uiGameplay.waitingWaveStartText.text = "Time left: " + i;
            yield return new WaitForSeconds(1);
        }
        uiGameplay.waitingWaveStartText.gameObject.SetActive(false);
    }
}