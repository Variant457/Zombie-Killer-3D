using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombie;
    public Transform playerTransform;
    public float maxSpawnRad = 50f;
    public float minSpawnRad = 10f;
    public float initSpawnInterval = 3f;

    //Profiler
    [SerializeField] private ProfilerDataExporter profilerExporter;

    void Start()
    {
        var stopwatch = Stopwatch.StartNew();
        StartCoroutine(SpawnZombieRoutine());
        stopwatch.Stop();
        profilerExporter?.RecordEvent("ZombieSpawner.Start", stopwatch.Elapsed.TotalMilliseconds);
    }

    void Update()
    {
        var stopwatch = Stopwatch.StartNew();
        if (Time.timeScale != 0)
        {
            if (initSpawnInterval >= 0) initSpawnInterval -= 0.00005f;
        }
        else initSpawnInterval = 3f;
        stopwatch.Stop();
        profilerExporter?.RecordEvent("ZombieSpawner.Update", stopwatch.Elapsed.TotalMilliseconds);
    }

    IEnumerator SpawnZombieRoutine()
    {
        while (true)
        {
            var stopwatch = Stopwatch.StartNew();
            if (Time.timeScale != 0)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                float randomDist = Random.Range(minSpawnRad, maxSpawnRad);
                Vector3 spawnPos = playerTransform.position + new Vector3(randomDir.x, 1, randomDir.y) * randomDist;
                Instantiate(zombie, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(initSpawnInterval);
            }

            stopwatch.Stop();
            profilerExporter?.RecordEvent("ZombieSpawner.SpawnZombieRoutine", stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
