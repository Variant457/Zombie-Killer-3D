using System.Diagnostics;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private ProfilerDataExporter profiler;

    void OnEnable()
    {
        profiler = FindAnyObjectByType<ProfilerDataExporter>();
    }

    public void Die()
    {
        var stopwatch = Stopwatch.StartNew();
        ObjectPool.Instance.ReturnObjectToPool(gameObject);
        stopwatch.Stop();
        profiler?.RecordEvent("Zombie.Die", stopwatch.Elapsed.TotalMilliseconds);
    }
}