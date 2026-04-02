using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int poolSize = 20;

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
        GameObject obj = Instantiate(coinPrefab, transform);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }

    public GameObject GetCoin()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        return CreateNew(); // expand pool
    }
}