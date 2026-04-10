using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform playerCarTransform;
    [SerializeField] CarController carController;

    [Header("Follow Settings")]
    //[SerializeField] float smoothSpeed = 5f;
    [SerializeField] Vector3 baseOffset = new Vector3(0f, 3f, -6f);

    [Header("Speed Zoom")]
    [SerializeField] float minDistance = 6f;
    [SerializeField] float maxDistance = 12f;

    [Header("Camera Angle")]
    [SerializeField] float tiltAngle = 10f; // X rotation

    private float currentDistance;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        currentDistance = minDistance;
    }

    private Vector3 moveVelocity;
    private float distanceVelocity;

    void LateUpdate()
    {
        if (playerCarTransform == null || carController == null)
            return;

        float speed = carController.CarSpeed();
        if (!float.IsFinite(speed)) speed = 0f;

        float t = Mathf.InverseLerp(0f, 100f, speed);

        float targetDistance = Mathf.Lerp(minDistance, maxDistance, t);

        currentDistance = Mathf.SmoothDamp(
            currentDistance,
            targetDistance,
            ref distanceVelocity,
            0.1f
        );

        if (!float.IsFinite(currentDistance))
            currentDistance = minDistance;

        Vector3 forward = playerCarTransform.forward;
        Vector3 position = playerCarTransform.position;

        // 🔥 HARD VALIDATION
        if (!float.IsFinite(forward.x) || !float.IsFinite(position.x))
            return;

        Vector3 desiredPosition = position
                                - forward * currentDistance
                                + Vector3.up * baseOffset.y;

        if (!float.IsFinite(desiredPosition.x))
            return;

        float camSmooth = Mathf.Lerp(0.08f, 0.03f, t);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref moveVelocity,
            camSmooth
        );

        Vector3 direction = position - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion tilt = Quaternion.Euler(tiltAngle, 0f, 0f);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation * tilt,
                Time.deltaTime * 6f
            );
        }
    }

    // 🔧 SETTERS (important for your spawner system)
    public void SetTransform(Transform target)
    {
        playerCarTransform = target;
    }

    public void SetCarController(CarController controller)
    {
        carController = controller;
    }
}