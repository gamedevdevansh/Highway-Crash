using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    //[SerializeField] Transform[] Lane;
    [SerializeField] Transform[] forwardLanes;
    [SerializeField] Transform[] oppositeLanes;
    [SerializeField] GameObject[] TrafficVehicle;
    [SerializeField] CarController carController;
    [SerializeField] float minSpawnTime = 30f;
    [SerializeField] float maxSpawnTime = 60f;
    private float dynamicTimer;
    [SerializeField] TrafficPool trafficPool;

    private Dictionary<Transform, float> laneLastSpawnTime = new Dictionary<Transform, float>();
    [SerializeField] float laneCooldown = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TrafficSpwaner());
    }

    public void SetCarController(CarController controller)
    {
        carController = controller;
    }

    IEnumerator TrafficSpwaner()
    {
        yield return new WaitForSeconds(4f);
        while (true)
        {
            if (carController.CarSpeed() > 20f)
            {
                //dynamicTimer = Random.Range(minSpawnTime, maxSpawnTime) / carController.CarSpeed();
                //dynamicTimer = Mathf.Lerp(2f, 0.5f, carController.CarSpeed() / 100f);
                dynamicTimer = Mathf.Lerp(maxSpawnTime, minSpawnTime, carController.CarSpeed() / 100f);
                SpawnTrafficVehicle();
            }
            else
            {
                dynamicTimer = 3f; // safe fallback
            }

            yield return new WaitForSeconds(dynamicTimer);
        }
    }

    //void SpawnTrafficVehicle()
    //{
    //    int randomLaneIndex = Random.Range(0, Lane.Length);
    //    int randomTrafficVehicleIndex = Random.Range(0, TrafficVehicle.Length);
    //    Instantiate(TrafficVehicle[randomLaneIndex], Lane[randomLaneIndex].position, Quaternion.identity);
    //}

    void SpawnTrafficVehicle()
    {
        //int randomVehicleIndex = Random.Range(0, TrafficVehicle.Length);

        bool spawnOpposite = Random.value > 0.5f;

        Transform[] spawnLanes;

        if (spawnOpposite)
        {
            spawnLanes = oppositeLanes;
        }
        else
        {
            spawnLanes = forwardLanes;
        }

        //GameObject vehicle = Instantiate(
        //    TrafficVehicle[randomVehicleIndex],
        //    spawnLane.position,
        //    Quaternion.identity
        //);

        // 👇 PICK ONE LANE (this is what you forgot)
        int randomLaneIndex = Random.Range(0, spawnLanes.Length);
        Transform lane = spawnLanes[randomLaneIndex];

        // 🚫 Prevent fast respawn in same lane
        if (laneLastSpawnTime.ContainsKey(lane))
        {
            if (Time.time - laneLastSpawnTime[lane] < laneCooldown)
                return;
        }

        // 🚫 Overlap check
        if (Physics.CheckSphere(lane.position, 2f))
            return;
        // ✅ Update last spawn time
        laneLastSpawnTime[lane] = Time.time;

        //GameObject vehicle = Instantiate(
        //    TrafficVehicle[randomVehicleIndex],
        //    lane.position,
        //    lane.rotation
        //);



        GameObject vehicle = trafficPool.GetVehicle();

        Vector3 spawnPos = lane.position;
        spawnPos.y += 0.5f;

        //vehicle.transform.position = lane.position;
        vehicle.transform.position = spawnPos;
        vehicle.transform.rotation = lane.rotation;
        vehicle.SetActive(true);


        TrafficVehicleAI ai = vehicle.GetComponent<TrafficVehicleAI>();

        if (ai != null)
        {
            ai.SetDirection(spawnOpposite ? -1 : 1);
        }
        //if (spawnOpposite)
        //{
        //    ai.SetDirection(-1); // coming towards player
        //}
        //else
        //{
        //    ai.SetDirection(1);  // same direction
        //}
    }
}