using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    //private void FixedUpdate()
    //{
    //    transform.Rotate(0, 0.15f, 0);
    //}

    void Update()
    {
        transform.Rotate(0, 90f * Time.deltaTime, 0);
    }
}
