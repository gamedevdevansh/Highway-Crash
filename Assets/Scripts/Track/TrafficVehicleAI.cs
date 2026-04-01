using UnityEngine;

public class TrafficVehicleAI : MonoBehaviour
{
    private float direction = 1f;
    [SerializeField] float minSpeed = 8f;
    [SerializeField] float maxSpeed = 15f;
    private float currentSpeed;

    [Header("Despawn Settings")]
    [SerializeField] float despawnDistanceBehind = 40f;
    [SerializeField] float despawnDistanceAhead = 200f; // Prevent fast cars from driving to infinity

    private Transform playerTransform;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }
    }

    public void SetDirection(float dir)
    {
        direction = dir;
    }

    void Update()
    {
        // Move the car
        transform.Translate(Vector3.forward * direction * currentSpeed * Time.deltaTime);

        // 👇 ENDLESS SYSTEM FIX: If the road chunk teleports and the car falls, recycle it instantly.
        if (transform.position.y < -5f)
        {
            gameObject.SetActive(false);
            return;
        }

        // 👇 RELIABLE DESPAWN: Check both ahead and behind
        if (playerTransform != null)
        {
            // Calculate distance relative to player
            float zDifference = transform.position.z - playerTransform.position.z;

            // If car is too far behind OR too far ahead
            if (zDifference < -despawnDistanceBehind || zDifference > despawnDistanceAhead)
            {
                gameObject.SetActive(false); // Return to pool!
            }
        }
        else
        {
            // Failsafe
            if (transform.position.z < -100f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}