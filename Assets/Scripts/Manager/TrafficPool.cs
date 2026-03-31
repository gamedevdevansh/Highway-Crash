//using System.Collections.Generic;
//using UnityEngine;

//public class TrafficPool : MonoBehaviour
//{
//    [SerializeField] GameObject[] vehiclePrefabs;
//    [SerializeField] int poolSize = 10;

//    private List<GameObject> pool = new List<GameObject>();

//    void Start()
//    {
//        for (int i = 0; i < poolSize; i++)
//        {
//            GameObject obj = Instantiate(
//                vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
//                transform
//            );
//            obj.SetActive(false);
//            pool.Add(obj);
//        }
//    }

//    //public GameObject GetVehicle()
//    //{
//    //    foreach (GameObject obj in pool)
//    //    {
//    //        if (!obj.activeInHierarchy)
//    //        {
//    //            return obj;
//    //        }
//    //    }

//    //    // optional: expand pool if needed
//    //    GameObject newObj = Instantiate(
//    //        vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
//    //        transform
//    //    );
//    //    newObj.SetActive(false);
//    //    pool.Add(newObj);
//    //    return newObj;
//    //}


//    public GameObject GetVehicle()
//    {
//        for (int i = 0; i < pool.Count; i++)
//        {
//            if (pool[i] == null)
//            {
//                // recreate destroyed object
//                GameObject newObj = Instantiate(
//                    vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
//                    transform
//                );
//                newObj.SetActive(false);
//                pool[i] = newObj;
//            }

//            if (!pool[i].activeInHierarchy)
//            {
//                return pool[i];
//            }
//        }

//        // expand pool
//        GameObject obj = Instantiate(
//            vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
//            transform
//        );
//        obj.SetActive(false);
//        pool.Add(obj);
//        return obj;
//    }
//}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a pool of reusable traffic vehicle instances to minimize Instantiate/Destroy overhead【29†L42-L47】.
/// </summary>
public class TrafficPool : MonoBehaviour
{
    [Tooltip("Prefabs of traffic vehicles to pool")]
    public GameObject[] vehiclePrefabs;
    [Tooltip("Initial number of pooled objects")]
    public int poolSize = 15;

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        // Pre-instantiate poolSize objects (inactive).
        for (int i = 0; i < poolSize; i++)
            CreateNewObject();
    }

    // Instantiate a new pooled object (inactive).
    private GameObject CreateNewObject()
    {
        var prefab = vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)];
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }

    /// <summary>
    /// Retrieves an inactive vehicle from the pool (or creates one if needed).
    /// </summary>
    public GameObject GetVehicle()
    {
        // Find first inactive object
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i] == null)
                return CreateNewObject();
            if (!pool[i].activeInHierarchy)
                return pool[i];
        }
        // All in use; expand pool
        return CreateNewObject();
    }
}
