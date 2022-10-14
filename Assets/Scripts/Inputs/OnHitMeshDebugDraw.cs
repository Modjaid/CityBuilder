using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitMeshDebugDraw : MonoBehaviour
{
    [SerializeField] private RaycastMeshCamera _raycast;

    private void Start()
    {
        _raycast.OnHit += OnHitMesh;
    }

    private void OnHitMesh(Transform collider, Vector3[] vertices)
    {
        vertices[0] = collider.TransformPoint(vertices[0]);
        vertices[1] = collider.TransformPoint(vertices[1]);
        vertices[2] = collider.TransformPoint(vertices[2]);
        Debug.DrawLine(vertices[0], vertices[1],Color.red);
        Debug.DrawLine(vertices[1], vertices[2],Color.red);
        Debug.DrawLine(vertices[2], vertices[0],Color.red);
    }
}
