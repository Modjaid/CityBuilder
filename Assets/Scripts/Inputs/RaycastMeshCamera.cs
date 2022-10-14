using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMeshCamera : MonoBehaviour
{
    public event Action<Transform, Vector3[]> OnHit;
    
    [SerializeField] private Camera _camera;

    // Update is called once per frame
    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);


        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit))
            return;

        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
            return;

        var vertices = meshCollider.sharedMesh.vertices;
        int[] triangles = meshCollider.sharedMesh.triangles;

        Vector3[] hitVertices = new Vector3[]
        {
            vertices[triangles[hit.triangleIndex * 3 + 0]],
            vertices[triangles[hit.triangleIndex * 3 + 1]],
            vertices[triangles[hit.triangleIndex * 3 + 2]]
        };
        
        OnHit?.Invoke(hit.collider.transform, hitVertices);
    }
}

