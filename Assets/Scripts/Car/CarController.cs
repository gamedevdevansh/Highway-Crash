using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssests.CrossPlatformInput;

public class CarController : MonoBehaviour
{
    // POWER UPS
    private bool isSpeedBoostActive = false;
    private bool isShieldActive = false;
    private bool isMagnetActive = false;

    [SerializeField] float boostMultiplier = 5f;
    [SerializeField] float boostDuration = 3f;

    private float originalTorque;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;

    public Transform frontRightWheelTransform;
    public Transform frontLeftWheelTransform;
    public Transform rearRightWheelTransform;
    public Transform rearLeftWheelTransform;

    public Transform carCentreOfMassTransform;
    public Rigidbody _rigidbody;

    public float torque = 200f;
    public float brakeForce = 200f;
    public float steeringAngle = 45f;

    float verticalInput;
    float horizontalInput;

    //public int GasInput;
    //public int BrakeInput;

    public bool GasInput;
    public bool BrakeInput;

    float originalDrag;

    [SerializeField] UIManager uiManager;

    [SerializeField] float highSpeedSteerMultiplier = 0.5f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody.centerOfMass = carCentreOfMassTransform.localPosition;
        originalTorque = torque;
        //_rigidbody.centerOfMass = new Vector3(0, -0.5f, 0);
    }
    public void SetUiManager(UIManager manager)
    {
        uiManager = manager;    
    }

    public void GasPressed()
    {
        GasInput = true;
    }
    public void GasReleased()
    {
        GasInput = false;
    }

    public void BrakePressed()
    {
        BrakeInput = true;
    }
    public void BrakeReleased()
    {
        BrakeInput = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        MotorForce();
        UpdateWheels();
        GetInput();
        Steering();
        ApplyingBrakes();
        //PowerSteering();
    }
    void GetInput()
    {
        // If touch is pressed, force verticalInput to 1.
        if (GasInput)
        {
            verticalInput = 1f;
        }
        else
        {
            // Only use keyboard if touch is NOT being used
            verticalInput = Input.GetAxis("Vertical");
        }

        // SimpleInput for steering is usually fine, but ensure 
        // it's not being fought by standard Input.GetAxis
        float touchHorizontal = SimpleInput.GetAxis("Horizontal");
        float keyboardHorizontal = Input.GetAxis("Horizontal");

        // Use whichever input is providing a stronger signal
        horizontalInput = Mathf.Abs(touchHorizontal) > 0.01f ? touchHorizontal : keyboardHorizontal;
    }
    void MotorForce()
    {
        float applied = torque * verticalInput;
        frontRightWheelCollider.motorTorque = applied;
        frontLeftWheelCollider.motorTorque = applied;
        rearRightWheelCollider.motorTorque = applied;
        rearLeftWheelCollider.motorTorque = applied;
    }

    //void Steering()
    //{
    //    frontRightWheelCollider.steerAngle = steeringAngle * horizontalInput;
    //    frontLeftWheelCollider.steerAngle = steeringAngle * horizontalInput;
    //}

    void Steering()
    {
        float speedFactor = Mathf.InverseLerp(0f, 100f, _rigidbody.velocity.magnitude * 2.236f);
        float steerReduce = Mathf.Lerp(1f, highSpeedSteerMultiplier, speedFactor);

        float steer = steeringAngle * steerReduce * horizontalInput;

        frontRightWheelCollider.steerAngle = steer;
        frontLeftWheelCollider.steerAngle = steer;
    }

    //void PowerSteering()
    //{
    //    if (horizontalInput == 0)
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 10f), Time.deltaTime);
    //    }
    //}


    void UpdateWheels()
    {
        RotateWheel(frontRightWheelCollider, frontRightWheelTransform);
        RotateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        RotateWheel(rearRightWheelCollider, rearRightWheelTransform);
        RotateWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    void RotateWheel(WheelCollider wheelCollider, Transform transform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        transform.rotation = rot;
    }

    void ApplyingBrakes()
    {
        bool isBraking = Input.GetKey(KeyCode.Space) || BrakeInput;

        if (isBraking)
        {
            frontRightWheelCollider.brakeTorque = brakeForce;
            frontLeftWheelCollider.brakeTorque = brakeForce;
            rearRightWheelCollider.brakeTorque = brakeForce;
            rearLeftWheelCollider.brakeTorque = brakeForce;
            _rigidbody.drag = .5f;
        }
        else
        {
            frontRightWheelCollider.brakeTorque = 0f;
            frontLeftWheelCollider.brakeTorque = 0f;
            rearRightWheelCollider.brakeTorque = 0f;
            rearLeftWheelCollider.brakeTorque = 0f;
            _rigidbody.drag = 0.02f;
        }
    }

    //public float CarSpeed()
    //{
    //    float speed = _rigidbody.velocity.magnitude * 2.23693629f;
    //    return speed;
    //}

    public float CarSpeed()
    {
        if (_rigidbody == null) return 0f;

        Vector3 v = _rigidbody.velocity;

        if (float.IsNaN(v.x) || float.IsInfinity(v.x) ||
            float.IsNaN(v.y) || float.IsInfinity(v.y) ||
            float.IsNaN(v.z) || float.IsInfinity(v.z))
        {
            return 0f;
        }

        return v.magnitude * 2.23693629f;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "TrafficVehicle")
    //    {
    //        uiManager.GameOver();
    //    }
    //}

    public void BoostSpeed()
    {
        if (!isSpeedBoostActive)
        {
            StartCoroutine(SpeedBoostCoroutine());
        }
    }

    //IEnumerator SpeedBoostCoroutine()
    //{
    //    isSpeedBoostActive = true;

    //    torque *= boostMultiplier;

    //    Debug.Log("SpeedBoost Activated");
    //    yield return new WaitForSeconds(boostDuration);

    //    torque = originalTorque;

    //    isSpeedBoostActive = false;
    //}

    IEnumerator SpeedBoostCoroutine()
    {
        isSpeedBoostActive = true;

        torque *= boostMultiplier;


        originalDrag = _rigidbody.drag;
        _rigidbody.drag = 0.05f; // less resistance


        // 🚀 FORCE SPEED BOOST
        _rigidbody.velocity *= 1.5f;

        Debug.Log("SpeedBoost Activated");

        yield return new WaitForSeconds(boostDuration);

        torque = originalTorque;

        isSpeedBoostActive = false;

        uiManager.ShowPowerUp("Speed", boostDuration);
    }

    public void ActivateShield()
    {
        isShieldActive = true;
        Debug.Log("Shield Activated");
        uiManager.ShowPowerUp("Shield", 5f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "TrafficVehicle")
        {
            if (isShieldActive)
            {
                isShieldActive = false;
                Debug.Log("Shield Used!");
                return;
            }

            uiManager.GameOver();
        }
    }
    public void ActivateMagnet()
    {
        if (!isMagnetActive)
        {
            StartCoroutine(MagnetCoroutine());
        }
    }

    public bool IsMagnetActive()
    {
        return isMagnetActive;
    }

    IEnumerator MagnetCoroutine()
    {
        isMagnetActive = true;

        Debug.Log("Magnet Activated");

        yield return new WaitForSeconds(5f);

        isMagnetActive = false;
        uiManager.ShowPowerUp("Magnet", 5f);
    }
}