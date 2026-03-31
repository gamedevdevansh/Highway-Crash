//using UnityEngine;

//public class TrafficVehicleAI : MonoBehaviour
//{
//    private float direction = 1f;
//    [SerializeField] float speed = 10f;
//    [SerializeField] float minSpeed = 8f;
//    [SerializeField] float maxSpeed = 15f;

//    //void Start()
//    //{
//    //    speed = Random.Range(minSpeed, maxSpeed);
//    //}
//    void OnEnable()
//    {
//        speed = Random.Range(minSpeed, maxSpeed);
//    }
//    public void SetDirection(float dir)
//    {
//        direction = dir;

//        //if (dir < 0)
//        //{
//        //    transform.rotation = Quaternion.Euler(0, 180, 0); // flip car
//        //}

//        //if (dir < 0)
//        //    transform.rotation = Quaternion.Euler(0, 180, 0);
//        //else
//        //    transform.rotation = Quaternion.identity;


//    }
//    void OnBecameInvisible()
//    {
//        gameObject.SetActive(false);
//    }
//    void Update()
//    {
//        transform.Translate(Vector3.forward * direction * speed * Time.deltaTime);
//    }
//}

using UnityEngine;

/// <summary>
/// Controls movement of a traffic vehicle. Uses OnEnable to reset state for pooling safety【29†L149-L152】.
/// </summary>
public class TrafficVehicleAI : MonoBehaviour
{
    [Tooltip("Min and Max randomized speed on spawn")]
    public float minSpeed = 8f;
    public float maxSpeed = 15f;

    private float speed;
    private float direction = 1f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        // Randomize speed each time this vehicle is (re)spawned
        speed = Random.Range(minSpeed, maxSpeed);

        // Reset physics velocities for safe reuse【29†L149-L152】 
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Set movement direction: 1 = forward lane, -1 = opposite lane.
    /// </summary>
    public void SetDirection(float dir)
    {
        direction = dir;
        // Note: we assume lane's transform.rotation already sets orientation【25†L70-L78】.
        // Do NOT apply additional Y-rotation here to avoid compounding errors.
    }

    void Update()
    {
        // Move the vehicle forward along its Z-axis each frame.
        // If true physics is needed, consider Rigidbody.MovePosition in FixedUpdate instead【25†L70-L78】.
        transform.Translate(Vector3.forward * direction * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        // Deactivate when not visible by any camera【20†L61-L69】 (returns to pool).
        gameObject.SetActive(false);
    }
}
