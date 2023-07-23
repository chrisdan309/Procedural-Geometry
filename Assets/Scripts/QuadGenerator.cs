using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadGenerator : MonoBehaviour
{
    private void Awake()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Quad";

        List<Vector3> points = new List<Vector3>()
        {
            new Vector3(-0.5f,0.5f),
            new Vector3(0.5f,0.5f),
            new Vector3(-0.5f,-0.5f),
            new Vector3(0.5f,-0.5f)
            
        };

        int[] triIndices = new int[]
        {
            2,1,0,
            3,1,2
        };

        List<Vector2> uvs = new List<Vector2>()
        {
            new Vector2(1,1),
            new Vector2(0,1),
            new Vector2(1,0),
            new Vector2(0,0)
        };
        List<Vector3> normals = new List<Vector3>()
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1)
        };

        mesh.SetVertices(points);
        mesh.SetNormals(normals);
        mesh.triangles = triIndices;
        mesh.SetUVs(0,uvs);

        mesh.RecalculateNormals();
        
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
