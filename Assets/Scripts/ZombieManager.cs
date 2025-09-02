using System.Diagnostics;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public float health = 50f;
    public float zombieSpeed = 3f;
    private float maxHealth;
    private GameManager gameManager;
    private Zombie zombieObj;

    //Profiler
    private ProfilerDataExporter profilerExporter;

    void Awake()
    {
        profilerExporter = FindAnyObjectByType<ProfilerDataExporter>();
        zombieObj = GetComponent<Zombie>();
        gameManager = FindAnyObjectByType<GameManager>();
        maxHealth = health;
    }

    void OnEnable()
    {
        health = maxHealth;
        GetComponent<ZombieHealthUI>().UpdateHealthUI(health, maxHealth);
        ZombieController.Instance.RegisterZombie(this);
    }

    void OnDisable()
    {
        ZombieController.Instance.UnregisterZombie(this);
    }

    public void TakeDmg(float amt)
    {
        var stopwatch = Stopwatch.StartNew();
        health -= amt;
        GetComponent<ZombieHealthUI>().UpdateHealthUI(health, maxHealth);

        if (health <= 0)
        {
            zombieObj.Die();
            gameManager.AddPoints();
        }

        stopwatch.Stop();
        profilerExporter?.RecordEvent("ZombieManager.TakeDmg", stopwatch.Elapsed.TotalMilliseconds);
    }
}
