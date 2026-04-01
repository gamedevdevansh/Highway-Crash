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

    private Dictionary<Transform, float> laneLastSpawnTime = new Dictionary<Transform, float>();

    private float dynamicTimer;

    void Start()
    {
        StartCoroutine(TrafficSpawner());
    }
    public void SetCarController(CarController controller)
    {
        carController = controller;
    }


    IEnumerator TrafficSpawner()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (carController != null && carController.CarSpeed() > 20f)
            {
                dynamicTimer = Mathf.Lerp(maxSpawnTime, minSpawnTime, carController.CarSpeed() / 100f);
                SpawnTrafficVehicle();
            }
            else
            {
                dynamicTimer = maxSpawnTime;
            }
            yield return new WaitForSeconds(dynamicTimer);
        }
    }

    void SpawnTrafficVehicle()
    {
        bool spawnOpposite = Random.value > 0.5f;
        Transform[] spawnLanes = spawnOpposite ? oppositeLanes : forwardLanes;

        // Pick one lane
        int randomLaneIndex = Random.Range(0, spawnLanes.Length);
        Transform lane = spawnLanes[randomLaneIndex];

        // Prevent fast respawn in same lane
        if (laneLastSpawnTime.ContainsKey(lane))
        {
            if (Time.time - laneLastSpawnTime[lane] < laneCooldown)
                return;
        }

        // Update last spawn time
        laneLastSpawnTime[lane] = Time.time;

        GameObject vehicle = trafficPool.GetVehicle();
        if (vehicle == null) return; // Safety check if pool is empty

        // Set Position
        //vehicle.transform.position = lane.position;

        float spawnDistance = 120f;

        Vector3 spawnPos = lane.position;
        spawnPos.z = carController.transform.position.z + spawnDistance;

        vehicle.transform.position = spawnPos;

        // 👇 THE FIX: Force the correct rotation here
        if (spawnOpposite)
        {
            // Rotate 180 degrees around the Y axis so headlights face the player
            vehicle.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            // Face the exact same way as the player
            vehicle.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        vehicle.SetActive(true);

        TrafficVehicleAI ai = vehicle.GetComponent<TrafficVehicleAI>();
        if (ai != null)
        {
            // 👇 ALWAYS pass 1! Because we rotated the car 180 degrees above, 
            // "driving forward" will automatically mean "driving towards the player".
            ai.SetDirection(1);
        }
    }
}