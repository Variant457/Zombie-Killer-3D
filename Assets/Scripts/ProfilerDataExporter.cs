using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

public class ProfilerDataExporter : MonoBehaviour
{
    private StringBuilder csvContent;
    private List<string> headerColumns;

    public string[] markersToRecord = new string[]
    {
        "GameManager.Start",
        "GameManager.GameOver",
        "GameManager.OnClick",
        "GameManager.AddPoints",
        "GunSystem.Update",
        "GunSystem.MyInput",
        "GunSystem.Shoot",
        "GunSystem.ResetShot",
        "GunSystem.Reload",
        "GunSystem.ReloadFinished",
        "HealthBarUI.Start",
        "HealthBarUI.UpdateHealthBar",
        "PlayerManager.Start",
        "PlayerManager.OnCollisionEnter",
        "PlayerManager.ResetHealth",
        "PlyerManager.Update",
        "ZombieHealthUI.Start",
        "ZombieHealthUI.LateUpdate",
        "ZombieHealthUI.UpdateHealthUI",
        "ZombieManager.Start",
        "ZombieManager.Update",
        "ZombieManager.TakeDmg",
        "ZombieSpawner.Start",
        "ZombieSpawner.Update",
        "ZombieSpawner.SpawnZombieRoutine"
    };

    void OnEnable()
    {
        csvContent = new StringBuilder();
        headerColumns = new List<string> { "Frame", "Time" };
        headerColumns.AddRange(markersToRecord.Select(m => $"\"{m} (ms)\""));

        csvContent.AppendLine(string.Join(",", headerColumns));
    }

    public void RecordEvent(string markerName, double ms)
    {
        var values = new string[headerColumns.Count];
        values[0] = Time.frameCount.ToString();
        values[1] = Time.time.ToString("F3");

        for (int i = 2; i < values.Length; i++)
        {
            values[i] = "0";
        }

        int markerIndex = headerColumns.IndexOf($"\"{markerName} (ms)\"");
        if (markerIndex > -1)
        {
            values[markerIndex] = ms.ToString("F6");
        }

        csvContent.AppendLine(string.Join(",", values));
    }

    public void WriteReportToFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"cpu_execution_report_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv");
        File.WriteAllText(filePath, csvContent.ToString());
        Debug.Log($"[ProfilerDataExporter] Report saved to: {filePath}");
    }

    void OnDisable()
    {
        WriteReportToFile();
    }
}