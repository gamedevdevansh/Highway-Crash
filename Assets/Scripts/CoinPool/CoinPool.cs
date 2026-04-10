using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int poolSize = 20;

    //private List<GameObject> pool = new List<GameObject>();
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(coinPrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    //GameObject CreateNew()
    //{
    //    GameObject obj = Instantiate(coinPrefab, transform);
    //    obj.SetActive(false);
    //    pool.Add(obj);
    //    return obj;
    //}

    //public GameObject GetCoin()
    //{
    //    foreach (GameObject obj in pool)
    //    {
    //        if (!obj.activeInHierarchy)
    //            return obj;
    //    }
    //    return CreateNew(); // expand pool
    //}

    public GameObject GetCoin()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            return obj;
        }

        GameObject newObj = Instantiate(coinPrefab, transform);
        return newObj;
    }

    public void ReturnCoin(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}