using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] Transform[] lanes;
    [SerializeField] CoinPool coinPool;
    [SerializeField] UIManager uiManager;
    [SerializeField] CarController carController;

    [SerializeField] float spawnDistance = 100f;
    [SerializeField] float spawnInterval = 2f;
    [SerializeField] int maxCoins = 50;
    int currentCoins = 0;

    float timer;

    //CarController carController;

    //void Start()
    //{
    //    carController = GameManager.Instance.CarController;
    //}

    //IEnumerator Start()
    //{
    //    ////yield return new WaitUntil(() => GameManager.Instance.CarController != null);
    //    //yield return new WaitUntil(() =>
    //    //    GameManager.Instance != null &&
    //    //    GameManager.Instance.CarController != null
    //    //);
    //    //carController = GameManager.Instance.CarController;
    //    ////StartCoroutine(SpawnCoins());
    //}

    //public void SetCarController(CarController controller)
    //{
    //    carController = controller;
    //}

    public void SetCarController(CarController controller)
    {
        carController = controller;

        if (carController == null)
        {
            Debug.LogError("CoinSpawner: CarController is NULL");
            return;
        }

        SpawnCoins();
    }
    void Update()
    {
        if (carController == null) return;
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnCoins();
            timer = 0f;
        }
    }

    void SpawnCoins()
    {
        if (currentCoins >= maxCoins) return;

        int laneIndex = Random.Range(0, lanes.Length);
        Transform lane = lanes[laneIndex];

        for (int i = 0; i < 3; i++)
        {
            GameObject coin = coinPool.GetCoin();

            Vector3 pos = lane.position;
            pos.y = 0f;
            pos.z = carController.transform.position.z + spawnDistance + (i * 3f);

            coin.transform.position = pos;

            Coin coinScript = coin.GetComponent<Coin>();
            coinScript.Init(carController.transform, carController, uiManager, coinPool, this);
            //coinScript.Init(carController.transform, carController, FindObjectOfType<UIManager>(), coinPool);

            coin.SetActive(true);
            currentCoins++;
        }
    }

    // 🔥 IMPORTANT: called when coin is collected
    public void ReturnCoin()
    {
        currentCoins--;
    }
}