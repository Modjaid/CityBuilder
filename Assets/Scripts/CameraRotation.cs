using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform target;
    public float speed;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
        transform.LookAt(target.position);
    }
}
