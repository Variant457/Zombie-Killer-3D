using System.Diagnostics;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public float health = 50f;
    public float zombieSpeed = 3f;
    private Transform playerTransform;
    private float maxHealth;

    //Profiler
    private ProfilerDataExporter profilerExporter;

    void Start()
    {
        profilerExporter = FindAnyObjectByType<ProfilerDataExporter>();
        var stopwatch = Stopwatch.StartNew();
        maxHealth = health;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        stopwatch.Stop();
        profilerExporter?.RecordEvent("ZombieManager.Start", stopwatch.Elapsed.TotalMilliseconds);
    }

    void Update()
    {
        var stopwatch = Stopwatch.StartNew();
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * zombieSpeed * Time.deltaTime;

        if (Time.timeScale != 0) zombieSpeed += 0.0003f;

        if (health <= 0)
        {
            Destroy(gameObject);
            FindAnyObjectByType<GameManager>().AddPoints();
        }

        stopwatch.Stop();
        profilerExporter?.RecordEvent("ZombieManager.Update", stopwatch.Elapsed.TotalMilliseconds);
    }

    public void TakeDmg(float amt)
    {
        var stopwatch = Stopwatch.StartNew();
        health -= amt;
        GetComponent<ZombieHealthUI>().UpdateHealthUI(health, maxHealth);
        stopwatch.Stop();
        profilerExporter?.RecordEvent("ZombieManager.TakeDmg", stopwatch.Elapsed.TotalMilliseconds);
    }
}
