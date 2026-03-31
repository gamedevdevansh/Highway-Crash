using UnityEngine;

public class TrafficVehicleAI : MonoBehaviour
{
    private float direction = 1f;
    [SerializeField] float speed = 10f;
    [SerializeField] float minSpeed = 8f;
    [SerializeField] float maxSpeed = 15f;

    //void Start()
    //{
    //    speed = Random.Range(minSpeed, maxSpeed);
    //}
    //void OnEnable()
    //{
    //    speed = Random.Range(minSpeed, maxSpeed);
    //}

    void OnEnable()
    {
        speed = Random.Range(minSpeed, maxSpeed);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    public void SetDirection(float dir)
    {
        direction = dir;

        //if (dir < 0)
        //{
        //    transform.rotation = Quaternion.Euler(0, 180, 0); // flip car
        //}

        //if (dir < 0)
        //    transform.rotation = Quaternion.Euler(0, 180, 0);
        //else
        //    transform.rotation = Quaternion.identity;


    }
    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * direction * speed * Time.deltaTime);
    }
}