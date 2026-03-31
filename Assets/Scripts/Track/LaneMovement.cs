//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LaneMovement : MonoBehaviour
//{
//    [SerializeField] Transform playerCarTransform;
//    [SerializeField] float offset = -5;
//    // Start is called before the first frame update
//    void Start()
//    {

//    }

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
//        Vector3 cameraPos = transform.position;
//        cameraPos.z = playerCarTransform.position.z + offset;
//        transform.position = cameraPos;
//    }
//}
using UnityEngine;

/// <summary>
/// Follows the player car to simulate an endless road behind the camera.
/// </summary>
public class LaneMovement : MonoBehaviour
{
    [Tooltip("Assign the player car Transform here")]
    public Transform playerCarTransform;
    [Tooltip("Distance behind the player to position the lane")]
    public float offset = -5f;

    public void SetTransform(Transform target)
    {
        playerCarTransform = target;
    }

    void LateUpdate()
    {
        if (playerCarTransform == null) return;
        Vector3 pos = transform.position;
        pos.z = playerCarTransform.position.z + offset;
        transform.position = pos;
    }
}
