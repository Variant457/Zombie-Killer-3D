using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HealthUIManager : MonoBehaviour
{
    public static List<Transform> healthUIs = new List<Transform>();
    private Transform cameraTransform;
    [SerializeField] private ProfilerDataExporter profiler;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        var stopwatch = Stopwatch.StartNew();
        Quaternion cameraRotation = cameraTransform.rotation;

        foreach (Transform uiTransform in healthUIs)
        {
            uiTransform.rotation = cameraRotation;
        }

        stopwatch.Stop();
        profiler?.RecordEvent("HealthUIManager.LateUpdate", stopwatch.Elapsed.TotalMilliseconds);
    }
}