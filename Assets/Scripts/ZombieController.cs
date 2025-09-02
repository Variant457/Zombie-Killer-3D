using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public static ZombieController Instance { get; private set; }
    public List<ZombieManager> allZombies = new List<ZombieManager>();
    private Transform playerTransform;

    [SerializeField] ProfilerDataExporter profiler;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        var stopwatch = Stopwatch.StartNew();
        foreach (ZombieManager zombie in allZombies)
        {
            Transform zombieTransform = zombie.transform;

            Vector3 direction = (playerTransform.position - zombieTransform.position).normalized;
            zombieTransform.position += direction * zombie.zombieSpeed * Time.deltaTime;

            if (Time.timeScale != 0) zombie.zombieSpeed += 0.00006f;
        }
        stopwatch.Stop();
        profiler?.RecordEvent("ZombieController.Update", stopwatch.Elapsed.TotalMilliseconds);
    }

    public void RegisterZombie(ZombieManager zombie)
    {
        var stopwatch = Stopwatch.StartNew();
        if (!allZombies.Contains(zombie))
        {
            allZombies.Add(zombie);
        }
        stopwatch.Stop();
        profiler?.RecordEvent("ZombieController.RegisterZombie", stopwatch.Elapsed.TotalMilliseconds);
    }

    public void UnregisterZombie(ZombieManager zombie)
    {
        var stopwatch = Stopwatch.StartNew();
        if (allZombies.Contains(zombie))
        {
            allZombies.Remove(zombie);
        }
        stopwatch.Stop();
        profiler?.RecordEvent("ZombieController.UnregisterZombie", stopwatch.Elapsed.TotalMilliseconds);
    }
}
