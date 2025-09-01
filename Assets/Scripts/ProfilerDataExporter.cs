using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Unity.Profiling;

public class ProfilerDataExporter : MonoBehaviour
{
    private StringBuilder csvContent;
    private List<string> headerColumns;

    //CPU Specific
    private ProfilerRecorder mainThreadTimeRecorder;

    // Memory Specific
    private ProfilerRecorder systemMemoryRecorder;
    private ProfilerRecorder gcMemoryRecorder;
    private ProfilerRecorder gcAllocRecorder;

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
            "Main Thread Time (ms)",
            "System Memory (MB)", "GC Memory (MB)", "GC Alloc (KB/s)"
        };
        headerColumns.AddRange(markersToRecord.Select(m => $"\"{m} (ms)\""));

        csvContent.AppendLine(string.Join(",", headerColumns));

        // CPU Recorders
        mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);

        // Memory Recorders
        systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        gcAllocRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Allocated In Frame");
        
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

        // CPU Stats
        values[2] = (mainThreadTimeRecorder.LastValue / 1000000.0).ToString("F2");

        // Memory Stats
        values[3] = (systemMemoryRecorder.LastValue / (1024.0 * 1024.0)).ToString("F2");
        values[4] = (gcMemoryRecorder.LastValue / (1024.0 * 1024.0)).ToString("F2");
        values[5] = (gcAllocRecorder.LastValue / 1024.0).ToString("F2");
        
        csvContent.AppendLine(string.Join(",", values));
    }

    public void WriteReportToFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"profiler_report_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv");
        File.WriteAllText(filePath, csvContent.ToString());
        Debug.Log($"[ProfilerDataExporter] Report saved to: {filePath}");
    }

    void OnDisable()
    {
        WriteReportToFile();

        // CPU Recorder Disposal
        mainThreadTimeRecorder.Dispose();

        // Memory Recorder Disposal
        systemMemoryRecorder.Dispose();
        gcMemoryRecorder.Dispose();
        gcAllocRecorder.Dispose();
    }
}