using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthUI : MonoBehaviour
{
    public Slider healthSlider;

    //Profiling
    private ProfilerDataExporter profilerExporter;

    void OnEnable()
    {
        HealthUIManager.healthUIs.Add(transform);
    }

    void OnDisable()
    {
        HealthUIManager.healthUIs.Remove(transform);
    }

    public void UpdateHealthUI(float update, float maxHealth)
    {
        var stopwatch = Stopwatch.StartNew();
        healthSlider.value = update / maxHealth;
        stopwatch.Stop();
        profilerExporter?.RecordEvent("ZombieHealthUI.UpdateHealthUI", stopwatch.Elapsed.TotalMilliseconds);
    }
}
