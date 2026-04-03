using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform player;
    private CarController carController;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float magnetRange = 10f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            carController = playerObj.GetComponent<CarController>();
        }
    }

    void Update()
    {
        if (player == null || carController == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 🧲 MAGNET EFFECT
        if (carController.IsMagnetActive() && distance < magnetRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;

            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Coin Collected");

        gameObject.SetActive(false); // pooling friendly
    }
}