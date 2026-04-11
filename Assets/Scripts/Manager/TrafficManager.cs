using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [Header("Lane References")]
    [SerializeField] private Transform[] forwardLanes;
    [SerializeField] private Transform[] oppositeLanes;

    [Header("Spawning Settings")]
    //[SerializeField] private float spawnDistanceAhead = 150f; // Lock spawn distance here
    [SerializeField] private float minSpawnTime = 0.5f;
    [SerializeField] private float maxSpawnTime = 3.0f;
    [SerializeField] private float laneCooldown = 1.5f;

    [Header("References")]
    [SerializeField] private TrafficPool trafficPool;
    [SerializeField] private CarController carController;

    //private Dictionary<Transform, float> laneLastSpawnTime = new Dictionary<Transform, float>();

    private float[] laneTimers;
    private Transform[] allLanes;
    bool isRunning = true;

    private float dynamicTimer;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance.CarController != null);
        carController = GameManager.Instance.CarController;

        allLanes = new Transform[forwardLanes.Length + oppositeLanes.Length];

        forwardLanes.CopyTo(allLanes, 0);
        oppositeLanes.CopyTo(allLanes, forwardLanes.Length);

        laneTimers = new float[allLanes.Length];

        StartCoroutine(TrafficSpawner());
    }
    public void SetCarController(CarController controller)
    {
        carController = controller;
    }

    IEnumerator TrafficSpawner()
    {
        yield return new WaitForSeconds(2f);

        while (isRunning)
        {
            float speed = carController != null ? carController.CarSpeed() : 0f;

            if (speed > 20f)
            {
                dynamicTimer = Mathf.Lerp(maxSpawnTime, minSpawnTime, speed / 100f);
                SpawnTrafficVehicle();
            }
            else
            {
                dynamicTimer = maxSpawnTime;
            }

            yield return new WaitForSeconds(dynamicTimer);
        }
    }
    //IEnumerator TrafficSpawner()
    //{
    //    yield return new WaitForSeconds(2f);

    //    while (isRunning)
    //    {
    //        if (carController != null && speed > 20f)
    //        {
    //            dynamicTimer = Mathf.Lerp(maxSpawnTime, minSpawnTime, speed / 100f);
    //            SpawnTrafficVehicle();
    //        }
    //        else
    //        {
    //            dynamicTimer = maxSpawnTime;
    //        }
    //        yield return new WaitForSeconds(dynamicTimer);
    //    }
    //}

    //void SpawnTrafficVehicle()
    //{
    //    bool spawnOpposite = Random.value > 0.5f;
    //    Transform[] spawnLanes = spawnOpposite ? oppositeLanes : forwardLanes;

    //    // Pick one lane
    //    int randomLaneIndex = Random.Range(0, spawnLanes.Length);
    //    Transform lane = spawnLanes[randomLaneIndex];

    //    // Prevent fast respawn in same lane
    //    if (laneLastSpawnTime.ContainsKey(lane))
    //    {
    //        if (Time.time - laneLastSpawnTime[lane] < laneCooldown)
    //            return;
    //    }

    //    // Update last spawn time
    //    laneLastSpawnTime[lane] = Time.time;

    //    GameObject vehicle = trafficPool.GetVehicle();
    //    if (vehicle == null) return; // Safety check if pool is empty

    //    // Set Position
    //    //vehicle.transform.position = lane.position;

    //    float spawnDistance = 120f;

    //    Vector3 spawnPos = lane.position;
    //    spawnPos.z = carController.transform.position.z + spawnDistance;

    //    vehicle.transform.position = spawnPos;

    //    // 👇 THE FIX: Force the correct rotation here
    //    if (spawnOpposite)
    //    {
    //        // Rotate 180 degrees around the Y axis so headlights face the player
    //        vehicle.transform.rotation = Quaternion.Euler(0, 180, 0);
    //    }
    //    else
    //    {
    //        // Face the exact same way as the player
    //        vehicle.transform.rotation = Quaternion.Euler(0, 0, 0);
    //    }

    //    vehicle.SetActive(true);

    //    TrafficVehicleAI ai = vehicle.GetComponent<TrafficVehicleAI>();
    //    if (ai != null)
    //    {
    //        // 👇 ALWAYS pass 1! Because we rotated the car 180 degrees above, 
    //        // "driving forward" will automatically mean "driving towards the player".
    //        ai.SetDirection(1);
    //    }
    //}

    void SpawnTrafficVehicle()
    {
        bool spawnOpposite = Random.value > 0.5f;
        Transform[] spawnLanes = spawnOpposite ? oppositeLanes : forwardLanes;

        int randomLaneIndex = Random.Range(0, spawnLanes.Length);
        Transform lane = spawnLanes[randomLaneIndex];

        int laneIndex = System.Array.IndexOf(allLanes, lane);

        if (Time.time - laneTimers[laneIndex] < laneCooldown)
            return;

        laneTimers[laneIndex] = Time.time;

        GameObject vehicle = trafficPool.GetVehicle();
        //if (vehicle == null) return;

        if (vehicle == null)
        {
            Debug.Log("Vehicle pool empty!");
            return;
        }

        float spawnDistance = 120f;

        Vector3 spawnPos = lane.position;
        spawnPos.z = carController.transform.position.z + spawnDistance;

        vehicle.transform.position = spawnPos;

        vehicle.transform.rotation = spawnOpposite
            ? Quaternion.Euler(0, 180, 0)
            : Quaternion.identity;

        vehicle.SetActive(true);

        //vehicle.TryGetComponent(out TrafficVehicleAI ai);
        vehicle.TryGetComponent(out TrafficVehicleAI ai);
        if (ai != null)
        {
            ai.Init(trafficPool);
            ai.SetDirection(1);
        }
        ai?.SetDirection(1);

        Debug.Log("Spawning traffic...");
    }
}