//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TrafficManager : MonoBehaviour
//{
//    //[SerializeField] Transform[] Lane;
//    [SerializeField] Transform[] forwardLanes;
//    [SerializeField] Transform[] oppositeLanes;
//    [SerializeField] GameObject[] TrafficVehicle;
//    [SerializeField] CarController carController;
//    [SerializeField] float minSpawnTime = 30f;
//    [SerializeField] float maxSpawnTime = 60f;
//    private float dynamicTimer ;
//    [SerializeField] TrafficPool trafficPool;

//// Start is called before the first frame update
//void Start()
//    {
//        StartCoroutine(TrafficSpwaner());
//    }

//    public void SetCarController(CarController controller)
//    {
//        carController = controller;
//    }

//    IEnumerator TrafficSpwaner()
//    {
//        yield return new WaitForSeconds(10f);
//        while (true)
//        {
//            if (carController.CarSpeed() > 20f)
//            {
//                dynamicTimer = Random.Range(minSpawnTime, maxSpawnTime) / carController.CarSpeed();
//                SpawnTrafficVehicle();
//            }
//            else
//            {
//                dynamicTimer = 3f; // safe fallback
//            }

//            yield return new WaitForSeconds(dynamicTimer);
//    }
//    }

//    //void SpawnTrafficVehicle()
//    //{
//    //    int randomLaneIndex = Random.Range(0, Lane.Length);
//    //    int randomTrafficVehicleIndex = Random.Range(0, TrafficVehicle.Length);
//    //    Instantiate(TrafficVehicle[randomLaneIndex], Lane[randomLaneIndex].position, Quaternion.identity);
//    //}

//    void SpawnTrafficVehicle()
//    {
//        //int randomVehicleIndex = Random.Range(0, TrafficVehicle.Length);

//        bool spawnOpposite = Random.value > 0.5f;

//        Transform[] spawnLanes;

//        if (spawnOpposite)
//        {
//            spawnLanes = oppositeLanes;
//        }
//        else
//        {
//            spawnLanes = forwardLanes;
//        }

//        //GameObject vehicle = Instantiate(
//        //    TrafficVehicle[randomVehicleIndex],
//        //    spawnLane.position,
//        //    Quaternion.identity
//        //);

//        // 👇 PICK ONE LANE (this is what you forgot)
//        int randomLaneIndex = Random.Range(0, spawnLanes.Length);
//        Transform lane = spawnLanes[randomLaneIndex];

//        //GameObject vehicle = Instantiate(
//        //    TrafficVehicle[randomVehicleIndex],
//        //    lane.position,
//        //    lane.rotation
//        //);



//        GameObject vehicle = trafficPool.GetVehicle();

//        vehicle.transform.position = lane.position;
//        vehicle.transform.rotation = lane.rotation;
//        vehicle.SetActive(true);


//    TrafficVehicleAI ai = vehicle.GetComponent<TrafficVehicleAI>();

//    if (ai != null)
//    {
//        ai.SetDirection(spawnOpposite ? -1 : 1);
//    }
//    //if (spawnOpposite)
//    //{
//    //    ai.SetDirection(-1); // coming towards player
//    //}
//    //else
//    //{
//    //    ai.SetDirection(1);  // same direction
//    //}
//}
//}
using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns traffic vehicles on forward and opposite lanes with pooling and safety checks.
/// </summary>
public class TrafficManager : MonoBehaviour
{
    [Header("Lane setup")]
    [Tooltip("Starting points for vehicles moving forward")]
    public Transform[] forwardLanes;
    [Tooltip("Starting points for vehicles moving opposite (toward player)")]
    public Transform[] oppositeLanes;

    [Header("Spawn timing (divided by speed)")]
    public float minSpawnTime = 100f;
    public float maxSpawnTime = 200f;
    public float spawnRadius = 2f;  // radius for overlap check

    [Header("Dependencies")]
    public TrafficPool trafficPool;
    public CarController playerCar;  // used for speed-check

    private float dynamicTimer;

    void Start()
    {
        StartCoroutine(TrafficSpawner());
    }

    public void SetPlayerCar(CarController controller)
    {
        playerCar = controller;
    }

    IEnumerator TrafficSpawner()
    {
        yield return new WaitForSeconds(3f); // initial delay
        while (true)
        {
            if (playerCar != null && playerCar.CarSpeed() > 20f)
            {
                // Scale spawn interval inversely with speed for increased density at higher speeds
                dynamicTimer = Random.Range(minSpawnTime, maxSpawnTime) / playerCar.CarSpeed();
                SpawnTrafficVehicle();
            }
            else
            {
                dynamicTimer = 2f; // slow speed or no player, use a minimal spawn delay
            }
            yield return new WaitForSeconds(dynamicTimer);
        }
    }

    void SpawnTrafficVehicle()
    {
        bool spawnOpposite = (Random.value > 0.5f);
        Transform[] lanes = spawnOpposite ? oppositeLanes : forwardLanes;

        if (lanes.Length == 0) return;
        int index = Random.Range(0, lanes.Length);
        Transform lane = lanes[index];

        // Avoid spawning on top of another vehicle
        if (Physics.CheckSphere(lane.position, spawnRadius))
            return;

        // Get pooled object
        GameObject vehicle = trafficPool.GetVehicle();

        // Set position (offset slightly to avoid ground clipping)
        Vector3 spawnPos = lane.position;
        spawnPos.y += 0.5f;
        vehicle.transform.position = spawnPos;
        vehicle.transform.rotation = lane.rotation;
        vehicle.SetActive(true);

        // Assign direction (-1 for opposite lanes, +1 for forward lanes)
        TrafficVehicleAI ai = vehicle.GetComponent<TrafficVehicleAI>();
        if (ai != null)
            ai.SetDirection(spawnOpposite ? -1f : 1f);
    }
}
