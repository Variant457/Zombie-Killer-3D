using System.Diagnostics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static float maxHealth = 100;
    public int dmgAmt = 5;
    private float health = maxHealth;

    [SerializeField]
    private HealthBarUI healthBar;
    [SerializeField]
    private GameManager gameManager;

    //Profiling
    [SerializeField] private ProfilerDataExporter profilerExporter;

    void Start()
    {
        gameManager.failScreen.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        var stopwatch = Stopwatch.StartNew();
        if (collision.gameObject.CompareTag("Enemy") && health > 0)
        {
            health -= dmgAmt;
            healthBar.UpdateHealthBar(health, maxHealth);
            if (health <= 0)
            {
                gameManager.GameOver();
            }
        }
        stopwatch.Stop();
        profilerExporter?.RecordEvent("PlayerManager.OnCollisionEnter", stopwatch.Elapsed.TotalMilliseconds);
    }

    public void ResetHealth()
    {
        var stopwatch = Stopwatch.StartNew();
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        stopwatch.Stop();
        profilerExporter?.RecordEvent("PlayerManager.ResetHealth", stopwatch.Elapsed.TotalMilliseconds);
    }

    // Update is called once per frame
    void Update()
    {
        var stopwatch = Stopwatch.StartNew();
        if (health > 0 && health < maxHealth)
        {
            health += 0.005f;
            healthBar.UpdateHealthBar(health, maxHealth);
        }
        stopwatch.Stop();
        profilerExporter?.RecordEvent("PlayerManager.Update", stopwatch.Elapsed.TotalMilliseconds);
    }
}
