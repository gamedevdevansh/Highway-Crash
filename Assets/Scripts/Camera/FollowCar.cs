using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    public Transform carTransform;
    public Transform cameraPointTransform;

    private Vector3 vector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    transform.LookAt(carTransform);
    //    transform.position = Vector3.SmoothDamp(transform.position, cameraPointTransform.position, ref vector, 0.05f);
    //}

    void LateUpdate()
    {
        transform.LookAt(carTransform);
        transform.position = Vector3.SmoothDamp(transform.position, cameraPointTransform.position, ref vector, 0.05f);
    }
}
