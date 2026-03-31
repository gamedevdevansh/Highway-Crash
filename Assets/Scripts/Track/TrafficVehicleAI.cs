using UnityEngine;

public class TrafficVehicleAI : MonoBehaviour
{
    private float direction = 1f;
    [SerializeField] float minSpeed = 8f;
    [SerializeField] float maxSpeed = 15f;
    private float currentSpeed;

    [Header("Despawn Settings")]
    [SerializeField] float despawnDistance = 40f; // Distance behind player to despawn
    private Transform playerTransform;
    private Rigidbody rb;

    void Awake()
    {
        // Cache the Rigidbody once so we don't use GetComponent multiple times
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);

        // Reset physics so it doesn't fly off when spawned
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Find the player every time it spawns to ensure we have the reference
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

        // RELIABLE DESPAWN: Check distance behind the player
        if (playerTransform != null)
        {
            // If the car's Z is less than the player's Z minus the despawn distance
            if (transform.position.z < playerTransform.position.z - despawnDistance)
            {
                gameObject.SetActive(false); // Return to pool!
            }
        }
        else
        {
            // Failsafe: If player gets destroyed, just despawn at a fixed negative Z
            if (transform.position.z < -100f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}