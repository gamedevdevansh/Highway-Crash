using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessCity : MonoBehaviour
{
    [SerializeField] Transform playerCarTransform;
    [SerializeField] Transform nextCityTransform;
    [SerializeField] float halfLength = 80f;
    public void SetTransform(Transform transform)
    {
        playerCarTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCarTransform.position.z > transform.position.z + halfLength + 50f)
        {
            transform.position = new Vector3(0, 0, nextCityTransform.position.z + halfLength * 2);
        }
    }
}
