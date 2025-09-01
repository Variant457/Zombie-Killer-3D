using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Unity.Profiling;

public class ProfilerDataExporter : MonoBehaviour
{
    // File Writing
    private StreamWriter reportWriter;
    private FileStream fileStream;
    private readonly object fileLock = new object();

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
        string filePath = Path.Combine(Application.persistentDataPath, $"profiler_report_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv");
        Debug.Log($"[ProfilerExporter] Report will be saved to: {filePath}");

        try
        {
            fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            reportWriter = new StreamWriter(fileStream);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[ProfilerExporter] Failed to open file for writing: {ex.Message}");
            return;
        }

        headerColumns = new List<string> {
            "Frame", "Time",
            "Main Thread Time (ms)",
            "System Memory (MB)", "GC Memory (MB)", "GC Alloc (KB/s)"
        };
        headerColumns.AddRange(markersToRecord.Select(m => $"\"{m} (ms)\""));

        reportWriter.WriteLine(string.Join(",", headerColumns));
        reportWriter.Flush();

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

        lock (fileLock) { reportWriter.WriteLine(string.Join(",", values)); }
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
        
        lock (fileLock) { reportWriter.WriteLine(string.Join(",", values)); }
    }

    void OnDisable()
    {
        Debug.Log("[ProfilerExporter] Closing Report File");
        lock (fileLock) { reportWriter?.Close(); }
        fileStream?.Close();

        // CPU Recorder Disposal
        mainThreadTimeRecorder.Dispose();

        // Memory Recorder Disposal
        systemMemoryRecorder.Dispose();
        gcMemoryRecorder.Dispose();
        gcAllocRecorder.Dispose();
    }
}