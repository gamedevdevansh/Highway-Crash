using System.Collections.Generic;
using UnityEngine;

public class PowerUpPool : MonoBehaviour
{
    [SerializeField] GameObject[] powerUpPrefabs;
    [SerializeField] int poolSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateNew();
        }
    }

    GameObject CreateNew()
    {
        GameObject obj = Instantiate(
            powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)],
            transform
        );

        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }

    public GameObject GetPowerUp()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        return CreateNew(); // expand pool
    }
}