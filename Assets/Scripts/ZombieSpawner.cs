using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Transform playerTransform;
    public float maxSpawnRad = 50f;
    public float minSpawnRad = 10f;
    public float initSpawnInterval = 3f;
    public float intervalDecrement = 0.05f;

    private WaitForSeconds spawnWait;

    //Profiler
    [SerializeField] private ProfilerDataExporter profilerExporter;

    void Start()
    {
        spawnWait = new WaitForSeconds(initSpawnInterval);
        StartCoroutine(SpawnZombieRoutine());
    }

    IEnumerator SpawnZombieRoutine()
    {
        while (true)
        {
            if (Time.timeScale != 0)
            {
                var stopwatch = Stopwatch.StartNew();

                GameObject zombieToSpawn = ObjectPool.Instance.GetPooledObject();

                if (zombieToSpawn != null)
                {
                    Vector2 randomDir = Random.insideUnitCircle.normalized;
                    float randomDist = Random.Range(minSpawnRad, maxSpawnRad);
                    Vector3 spawnPos = playerTransform.position + new Vector3(randomDir.x, 1, randomDir.y) * randomDist;
                    
                    zombieToSpawn.transform.position = spawnPos;
                    zombieToSpawn.transform.rotation = Quaternion.identity;
                    zombieToSpawn.SetActive(true);
                }

                if (initSpawnInterval > 0.06f)
                {
                    initSpawnInterval -= intervalDecrement;
                    spawnWait = new WaitForSeconds(initSpawnInterval);
                }

                stopwatch.Stop();
                profilerExporter?.RecordEvent("ZombieSpawner.SpawnZombieRoutine", stopwatch.Elapsed.TotalMilliseconds);

                yield return spawnWait;
            }
            else yield return null;
        }
    }
}
