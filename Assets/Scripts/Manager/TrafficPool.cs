using System.Collections.Generic;
using UnityEngine;

public class TrafficPool : MonoBehaviour
{
    [SerializeField] GameObject[] vehiclePrefabs;
    [SerializeField] int poolSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        if (vehiclePrefabs.Length == 0)
        {
            Debug.LogError("TrafficPool: No vehicle prefabs assigned!");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewVehicleAndAddToPool();
        }
    }

    public GameObject GetVehicle()
    {
        if (vehiclePrefabs.Length == 0) return null;

        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i] == null)
            {
                // Recreate destroyed object
                pool[i] = CreateNewVehicleAndAddToPool(false);
            }

            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        // Expand pool if all cars are currently on the road
        return CreateNewVehicleAndAddToPool(false);
    }

    // Helper method to keep code clean
    private GameObject CreateNewVehicleAndAddToPool(bool addToList = true)
    {
        GameObject obj = Instantiate(
            vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
            transform
        );
        obj.SetActive(false);
        if (addToList) pool.Add(obj);
        return obj;
    }
}