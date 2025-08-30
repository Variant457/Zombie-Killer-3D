using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombie;
    public Transform playerTransform;
    public float maxSpawnRad = 50f;
    public float minSpawnRad = 10f;
    public float initSpawnInterval = 3f;

    void Start()
    {
        StartCoroutine(SpawnZombieRoutine());
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (initSpawnInterval >= 0) initSpawnInterval -= 0.00005f;
        }
        else initSpawnInterval = 3f;
    }

    IEnumerator SpawnZombieRoutine() {
        while (true)
        {
            if (Time.timeScale != 0)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                float randomDist = Random.Range(minSpawnRad, maxSpawnRad);
                Vector3 spawnPos = playerTransform.position + new Vector3(randomDir.x, 1, randomDir.y) * randomDist;
                Instantiate(zombie, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(initSpawnInterval);
            }
        }
    }
}
