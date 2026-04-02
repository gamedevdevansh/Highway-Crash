using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] Transform[] lanes;
    [SerializeField] CoinPool coinPool;
    [SerializeField] CarController carController;

    [SerializeField] float spawnDistance = 100f;
    [SerializeField] float spawnInterval = 2f;

    float timer;


    public void SetCarController(CarController controller)
    {
        carController = controller;
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnCoins();
            timer = 0f;
        }
    }

    void SpawnCoins()
    {
        if (carController == null) return;

        int laneIndex = Random.Range(0, lanes.Length);
        Transform lane = lanes[laneIndex];

        // 🔥 spawn 3 coins in a line
        for (int i = 0; i < 3; i++)
        {
            GameObject coin = coinPool.GetCoin();

            Vector3 pos = lane.position;
            pos.y = 1f;
            pos.z = carController.transform.position.z + spawnDistance + (i * 3f);

            coin.transform.position = pos;
            coin.transform.rotation = lane.rotation;

            coin.SetActive(true);
        }
    }
}