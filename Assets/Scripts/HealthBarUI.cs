using System.Diagnostics;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBar;

    private float width;

    //Profiling
    [SerializeField] private ProfilerDataExporter profilerExporter;

    void Start()
    {
        width = healthBar.sizeDelta.x;
    }

    public void UpdateHealthBar(float currentHP, float maxHP)
    {
        var stopwatch = Stopwatch.StartNew();
        float healthPer = currentHP / maxHP;
        float newWidth = width * healthPer;

        healthBar.sizeDelta = new Vector2(newWidth, healthBar.sizeDelta.y);
        stopwatch.Stop();
        profilerExporter?.RecordEvent("HealthBarUI.UpdateHealthBar", stopwatch.Elapsed.TotalMilliseconds);
    }
}
