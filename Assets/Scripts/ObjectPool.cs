using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public GameObject objectToPool;
    public int amountToPool;
    private Queue<GameObject> pooledObjects;

    [SerializeField] private ProfilerDataExporter profiler;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        var stopwatch = Stopwatch.StartNew();
        pooledObjects = new Queue<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Enqueue(obj);
        }

        stopwatch.Stop();
        profiler?.RecordEvent("ObjectPool.Start", stopwatch.Elapsed.TotalMilliseconds);
    }

    public GameObject GetPooledObject()
    {
        var stopwatch = Stopwatch.StartNew();

        if (pooledObjects.Count > 0)
        {
            GameObject obj = pooledObjects.Dequeue();

            stopwatch.Stop();
            profiler?.RecordEvent("ObjectPool.GetPooledObject", stopwatch.Elapsed.TotalMilliseconds);
            return obj;
        }

        GameObject newObj = Instantiate(objectToPool);

        stopwatch.Stop();
        profiler?.RecordEvent("ObjectPool.GetPooledObject", stopwatch.Elapsed.TotalMilliseconds);
        return newObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        pooledObjects.Enqueue(obj);
    }
}