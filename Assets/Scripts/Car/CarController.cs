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
        PowerSteering();
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

    void Steering()
    {
        frontRightWheelCollider.steerAngle = steeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steeringAngle * horizontalInput;
    }

    void PowerSteering()
    {
        if (horizontalInput == 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 10f), Time.deltaTime);
        }
    }


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
            _rigidbody.drag = 1f;
        }
        else
        {
            frontRightWheelCollider.brakeTorque = 0f;
            frontLeftWheelCollider.brakeTorque = 0f;
            rearRightWheelCollider.brakeTorque = 0f;
            rearLeftWheelCollider.brakeTorque = 0f;
            _rigidbody.drag = 0.1f;
        }
    }

    public float CarSpeed()
    {
        float speed = _rigidbody.velocity.magnitude * 2.23693629f;
        return speed;
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
    }

    public void ActivateShield()
    {
        isShieldActive = true;

        Debug.Log("Shield Activated");
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
    }
}