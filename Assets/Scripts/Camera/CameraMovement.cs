//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CameraMovement : MonoBehaviour
//{
//    [SerializeField] Transform playerCarTransform;
//    [SerializeField] float offset = -5;

//    public void SetTransform(Transform transform)
//    {
//        playerCarTransform = transform;
//    }

//    // Update is called once per frame
//    void LateUpdate()
//    {
//        if (playerCarTransform == null)
//        {
//            return;
//        }
//        Vector3 cameraPos =  transform.position;
//        cameraPos.z = playerCarTransform.position.z + offset;
//        transform.position = cameraPos;
//    }
//}
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform playerCarTransform;
    [SerializeField] CarController carController;

    [SerializeField] float minOffset = -5f;   // close camera
    [SerializeField] float maxOffset = -12f;  // far camera

    [SerializeField] float smoothSpeed = 5f;

    private float currentOffset;

    public void SetTransform(Transform transform)
    {
        playerCarTransform = transform;
    }

    public void SetCarController(CarController controller)
    {
        carController = controller;
    }

    void LateUpdate()
    {
        if (playerCarTransform == null || carController == null)
            return;

        float speed = carController.CarSpeed();

        // Normalize speed (0 → 100 km/h)
        float t = Mathf.InverseLerp(0, 100f, speed);

        // Calculate dynamic offset
        float targetOffset = Mathf.Lerp(minOffset, maxOffset, t);

        // Smooth transition
        currentOffset = Mathf.Lerp(currentOffset, targetOffset, Time.deltaTime * smoothSpeed);

        Vector3 cameraPos = transform.position;
        cameraPos.z = playerCarTransform.position.z + currentOffset;

        transform.position = cameraPos;
    }
}