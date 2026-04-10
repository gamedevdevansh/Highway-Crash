using System.Collections.Generic;
using UnityEngine;

public class TrafficPool : MonoBehaviour
{
    [SerializeField] GameObject[] vehiclePrefabs;
    [SerializeField] int poolSize = 10;
    [SerializeField] int maxPoolSize = 30;

    //private List<GameObject> pool = new List<GameObject>();
    Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        if (vehiclePrefabs.Length == 0)
        {
            Debug.LogError("TrafficPool: No vehicle prefabs assigned!");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            //CreateNewVehicleAndAddToPool();
            GameObject obj = CreateNewVehicle();
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    //public GameObject GetVehicle()
    //{
    //    if (vehiclePrefabs.Length == 0) return null;

    //    for (int i = 0; i < pool.Count; i++)
    //    {
    //        if (pool[i] == null)
    //        {
    //            // Recreate destroyed object
    //            pool[i] = CreateNewVehicleAndAddToPool(false);
    //        }

    //        if (!pool[i].activeInHierarchy)
    //        {
    //            return pool[i];
    //        }
    //    }

    //    // Expand pool if all cars are currently on the road
    //    return CreateNewVehicleAndAddToPool(true);
    //}

    public GameObject GetVehicle()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }

        return CreateNewVehicle();
    }

    public void ReturnVehicle(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    private GameObject CreateNewVehicle()
    {
        GameObject obj = Instantiate(
            vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
            transform
        );

        obj.SetActive(false);
        return obj;
    }
    // Helper method to keep code clean
    //private GameObject CreateNewVehicleAndAddToPool(bool addToList = true)
    //{
    //    GameObject obj = Instantiate(
    //        vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
    //        transform
    //    );
    //    obj.SetActive(false);
    //    if (addToList) pool.Add(obj);
    //    return obj;
    //}
}