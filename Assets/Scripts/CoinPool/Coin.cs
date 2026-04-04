//using UnityEngine;
//using static UnityEditor.Timeline.TimelinePlaybackControls;

//public class Coin : MonoBehaviour
//{
//    private Transform player;
//    private CarController carController;

//    [SerializeField] float moveSpeed = 10f;
//    [SerializeField] float magnetRange = 10f;
//    private UIManager uiManager;
//    private bool isCollected = false;

//    void OnEnable()
//    {
//        isCollected = false;
//    }
//    void Start()
//    {
//        uiManager = FindObjectOfType<UIManager>();
//        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//        if (playerObj != null)
//        {
//            player = playerObj.transform;
//            carController = playerObj.GetComponent<CarController>();
//        }
//    }

//    void Update()
//    {
//        if (player == null || carController == null) return;

//        float distance = Vector3.Distance(transform.position, player.position);

//        // 🧲 MAGNET EFFECT
//        if (carController.IsMagnetActive() && distance < magnetRange)
//        {
//            Vector3 direction = (player.position - transform.position).normalized;

//            transform.position += direction * moveSpeed * Time.deltaTime;
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (isCollected) return;

//        if (!other.CompareTag("Player")) return;

//        Debug.Log("Coin Collected");

//        isCollected = true;

//        uiManager.AddCoin(1);

//        gameObject.SetActive(false); // pooling friendly
//    }
//}

using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform player;
    private CarController carController;
    private UIManager uiManager;

    [SerializeField] float magnetRange = 10f;
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] float collectDistance = 1.5f;

    private bool isCollected = false;

    void OnEnable()
    {
        isCollected = false;
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            carController = playerObj.GetComponent<CarController>();
            uiManager = FindObjectOfType<UIManager>();
        }
    }

    void Update()
    {
        if (player == null || carController == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 🧲 MAGNET MOVEMENT
        if (carController.IsMagnetActive() && distance < magnetRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;

            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // ✅ NEW: AUTO COLLECT (VERY IMPORTANT)
        if (!isCollected && distance < collectDistance)
        {
            Collect();
        }
    }

    void Collect()
    {
        isCollected = true;

        if (uiManager != null)
        {
            uiManager.AddCoin(1);
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (!other.CompareTag("Player")) return;

        Collect();
    }
}