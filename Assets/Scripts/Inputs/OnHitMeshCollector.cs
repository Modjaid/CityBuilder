using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitMeshCollector : MonoBehaviour
{
    [SerializeField] private RaycastMeshCamera _raycast;
    [SerializeField] private MeshFilter _meshFilter;

    private List<Vector3> _vertices;
    private List<int> _tris;
    private Mesh _mesh;
    private void Start()
    {
        _raycast.OnHit += OnHitMesh;
        _vertices = new List<Vector3>();
        _tris = new List<int>();
        _mesh = _meshFilter.mesh;
    }
    
    private void OnHitMesh(Transform collider, Vector3[] vertices)
    {
        if (!_vertices.Contains(vertices[0]))
        {
            AddTriangle(vertices);
            UpdateMesh();
        }
    }
    
    private void UpdateMesh()
    {
        // Mesh mesh = _meshFilter.mesh;
        Debug.Log($"_tris: {_tris.Count}");
        Debug.Log($"_vertices: {_vertices.Count}");

        _mesh.Clear();
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _tris.ToArray();
        _mesh.RecalculateNormals();
    }

    private void AddTriangle(Vector3[] vertices)
    {
        int i = vertices.Length;

        _tris.AddRange(new int[]
        {
            i,
            i + 1,
            i + 2,
            i + 3
        });

        _vertices.AddRange(vertices);
    }

    private void OnDrawGizmos()
    {
        //     foreach (var triangle in _tris)
        //     {
        //         
        //     }
        
    }
}
