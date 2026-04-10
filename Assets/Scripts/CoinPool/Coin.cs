using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform player;
    private CarController carController;
    private UIManager uiManager;
    private CoinPool pool;

    [SerializeField] float magnetRange = 30f;
    [SerializeField] float moveSpeed = 15f;
    [SerializeField] float collectDistance = 1.5f;

    private bool isCollected = false;

    void OnEnable()
    {
        isCollected = false;
    }


    //void Start()
    //{
    //    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

    //    if (playerObj != null)
    //    {
    //        player = playerObj.transform;
    //        carController = playerObj.GetComponent<CarController>();
    //        //uiManager = FindObjectOfType<UIManager>();
    //    }
    //}

    public void Init(Transform playerRef, CarController carCtrl, UIManager ui, CoinPool poolRef)
    {
        player = playerRef;
        carController = carCtrl;
        uiManager = ui;
        pool = poolRef;
    }

    void Update()
    {
        if (player == null || carController == null) return;

        //float distance = Vector3.Distance(transform.position, player.position);
        float sqrDist = (transform.position - player.position).sqrMagnitude;

        // 🧲 MAGNET MOVEMENT
        //if (carController.IsMagnetActive() && distance < magnetRange)
        if (carController.IsMagnetActive() && sqrDist < magnetRange * magnetRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;

            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // ✅ NEW: AUTO COLLECT (VERY IMPORTANT)
        //if (!isCollected && distance < collectDistance)
        if (!isCollected && sqrDist < collectDistance * collectDistance)
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

        //gameObject.SetActive(false);
        pool.ReturnCoin(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (!other.CompareTag("Player")) return;

        Collect();
    }
}