using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Unity.Profiling;
using Unity.VisualScripting.Dependencies.NCalc;

public class ProfilerDataExporter : MonoBehaviour
{
    private StringBuilder csvContent;
    private List<string> headerColumns;

    // Memory Specific
    private ProfilerRecorder systemMemoryRecorder;
    private ProfilerRecorder gcMemoryRecorder;
    private ProfilerRecorder mainThreadTimeRecorder;
    private float statsRecordInterval = 1.0f;
    private float timeSinceLastRecord = 0f;

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
        headerColumns = new List<string> {
            "Frame", "Time",
            "System Memory (MB)", "GC Memory (MB)", "Main Thread Time (ms)"
        };
        headerColumns.AddRange(markersToRecord.Select(m => $"\"{m} (ms)\""));

        csvContent.AppendLine(string.Join(",", headerColumns));

        systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
    }

    void Update()
    {
        timeSinceLastRecord += Time.deltaTime;
        if (timeSinceLastRecord >= statsRecordInterval)
        {
            timeSinceLastRecord -= statsRecordInterval;
            RecordGeneralStats();
        }
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

    private void RecordGeneralStats()
    {
        var values = new string[headerColumns.Count];
        for (int i = 0; i < values.Length; i++) { values[i] = "0"; }

        values[0] = Time.frameCount.ToString();
        values[1] = Time.time.ToString("F3");

        values[2] = (systemMemoryRecorder.LastValue / (1024.0 * 1024.0)).ToString("F2");
        values[3] = (gcMemoryRecorder.LastValue / (1024.0 * 1024.0)).ToString("F2");
        values[4] = (mainThreadTimeRecorder.LastValue / 1000000.0).ToString("F2");

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

        systemMemoryRecorder.Dispose();
        gcMemoryRecorder.Dispose();
        mainThreadTimeRecorder.Dispose();
    }
}