using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDebug : MonoBehaviour
{
    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);


        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log($"x:{hit.point.x} y:{hit.point.z}");
        }
    }
}
