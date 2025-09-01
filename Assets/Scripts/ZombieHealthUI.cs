using System.Diagnostics;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class ZombieHealthUI : MonoBehaviour
{
    private Transform cameraTransform;
    public Slider healthSlider;

    //Profiling
    private ProfilerDataExporter profilerExporter;

    void Start()
    {
        profilerExporter = FindAnyObjectByType<ProfilerDataExporter>();
        var stopwatch = Stopwatch.StartNew();
        cameraTransform = Camera.main.transform;
        stopwatch.Stop();
        profilerExporter?.RecordEvent("ZombieHealthUI.Start", stopwatch.Elapsed.TotalMilliseconds);
    }

    void LateUpdate()
    {
        var stopwatch = Stopwatch.StartNew();
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);
        stopwatch.Stop();
        profilerExporter?.RecordEvent("ZombieHealthUI.LateUpdate", stopwatch.Elapsed.TotalMilliseconds);
    }

    public void UpdateHealthUI(float update, float maxHealth)
    {
        var stopwatch = Stopwatch.StartNew();
        healthSlider.value = update / maxHealth;
        stopwatch.Stop();
        profilerExporter?.RecordEvent("ZombieHealthUI.UpdateHealthUI", stopwatch.Elapsed.TotalMilliseconds);
    }
}
