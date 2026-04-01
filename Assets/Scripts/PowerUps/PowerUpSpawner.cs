using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] Transform[] lanes;
    [SerializeField] PowerUpPool powerUpPool;
    [SerializeField] Transform player;

    [SerializeField] float spawnDistance = 120f;
    [SerializeField] float spawnInterval = 3f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPowerUp();
            timer = 0f;
        }
    }

    void SpawnPowerUp()
    {
        int laneIndex = Random.Range(0, lanes.Length);
        Transform lane = lanes[laneIndex];

        GameObject powerUp = powerUpPool.GetPowerUp();

        Vector3 pos = lane.position;
        pos.z = player.position.z + spawnDistance;

        powerUp.transform.position = pos;
        powerUp.transform.rotation = lane.rotation;

        powerUp.SetActive(true);
    }
}