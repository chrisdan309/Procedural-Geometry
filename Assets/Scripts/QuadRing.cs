using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class QuadRing : MonoBehaviour
{
    [Range(0.01f, 2)]
    [SerializeField] float radiusInner;
    [Range(0.01f, 2)]
    [SerializeField] float thickness;
    [Range(3,256)]
    [SerializeField] int angularSegmentCount = 3;

    private Mesh _mesh;
    private float RadiusOuter => radiusInner + thickness;
    private int VertexCount => angularSegmentCount * 2;

    private void OnDrawGizmosSelected()
    {
        Gizmosfs.DrawWireCircle(transform.position, transform.rotation,radiusInner, angularSegmentCount);
        Gizmosfs.DrawWireCircle(transform.position, transform.rotation,RadiusOuter, angularSegmentCount);
    }

    private void Awake()
    {
        _mesh = new Mesh();
        _mesh.name = "QuadRing";
        GetComponent<MeshFilter>().sharedMesh = _mesh;
        
    }

    void Update() => GenerateMesh();
    
    void GenerateMesh()
    {
        _mesh.Clear();

        int vCount = VertexCount;
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        
        for (int i = 0; i < angularSegmentCount; i++)
        {
            float t = i / (float )angularSegmentCount;
            float angRad = t * Mathfs.Tau;
            Vector2 dir = Mathfs.GetUnitVectorByAngle(angRad);
            vertices.Add(dir * RadiusOuter);
            vertices.Add(dir * radiusInner);
            
            normals.Add((Quaternion.Inverse(transform.rotation)* Vector3.forward).normalized);
            normals.Add((Quaternion.Inverse(transform.rotation)* Vector3.forward).normalized);
        }

    

        List<int> triangleIndices = new List<int>();
        
        for (int i = 0; i < angularSegmentCount; i++)
        {
            int indexRoot = i * 2;
            int indexInnerRoot = indexRoot + 1;
            int indexOuterNext = (indexRoot + 2) % vCount;
            int indexInnerNext = (indexRoot + 3) % vCount;
            
            triangleIndices.Add(indexRoot);
            triangleIndices.Add(indexInnerNext);
            triangleIndices.Add(indexInnerRoot);
            
            triangleIndices.Add(indexRoot);
            triangleIndices.Add(indexOuterNext);
            triangleIndices.Add(indexInnerNext);
        }
        
        
        _mesh.SetVertices(vertices);
        _mesh.SetTriangles(triangleIndices, 0);
        _mesh.SetNormals(normals);
    }
}
