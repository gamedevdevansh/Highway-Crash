using System.Collections.Generic;
using UnityEngine;

public class TrafficPool : MonoBehaviour
{
    [SerializeField] GameObject[] vehiclePrefabs;
    [SerializeField] int poolSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(
                vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
                transform
            );
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    //public GameObject GetVehicle()
    //{
    //    foreach (GameObject obj in pool)
    //    {
    //        if (!obj.activeInHierarchy)
    //        {
    //            return obj;
    //        }
    //    }

    //    // optional: expand pool if needed
    //    GameObject newObj = Instantiate(
    //        vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
    //        transform
    //    );
    //    newObj.SetActive(false);
    //    pool.Add(newObj);
    //    return newObj;
    //}


    public GameObject GetVehicle()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i] == null)
            {
                // recreate destroyed object
                GameObject newObj = Instantiate(
                    vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
                    transform
                );
                newObj.SetActive(false);
                pool[i] = newObj;
            }

            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        // expand pool
        GameObject obj = Instantiate(
            vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)],
            transform
        );
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }
}