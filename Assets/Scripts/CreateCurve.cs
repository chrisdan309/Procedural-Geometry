using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.ComponentModel.Design;
using System.IO;

public class CreateCurve : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float T;
    [Range(2,32)]
    public int roadSegments = 4;

    private int controlPointsCount = 4;
    private List<Transform> controlPoints = new List<Transform>();
    GameObject sphere;
       
    Mesh mesh;

    private void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Procedural Road";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    void Start()
    {
        T = 0;
        controlPoints.Clear();
       for (int i = 0; i < controlPointsCount; i++)
        {
            controlPoints.Add(gameObject.transform.GetChild(i));
        }
       controlPointsCount = gameObject.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        mesh.Clear();
        List<Vector3> verts = new List<Vector3>();
        for(int i =0; i < roadSegments; i++) { 
            OrientedPoint op = GetBezierOn((float)i / (roadSegments-1.0f));
            verts.Add(op.LocalToWorld(Vector3.right * -1.5f));
            verts.Add(op.LocalToWorld(-Vector3.up * 0.2f + Vector3.right * -1.0f));
            verts.Add(op.LocalToWorld(-Vector3.up * 0.2f + Vector3.right * 1.0f));
            verts.Add(op.LocalToWorld(Vector3.right * 1.5f));
        }
        List<int> triangles = new List<int>();
        for (int i = 0; i < roadSegments - 1; i++)
        {
            int root = i * 4;
            int rootNext = (i + 1) * 4;
            
            triangles.Add(root);
            triangles.Add(rootNext);
            triangles.Add(root + 3);
            triangles.Add(rootNext);
            triangles.Add(rootNext + 3);
            triangles.Add(root + 3);
        }
        mesh.SetVertices(verts);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Gizmos.DrawSphere(gameObject.transform.GetChild(i).position, 0.05f);
        }
        Gizmos.color = Color.cyan;
        Vector3[] verts = new Vector3[4];
        int[] vertsIndex = new int[4];
        vertsIndex[0] = 1;
        vertsIndex[1] = 2;
        vertsIndex[2] = 3;
        vertsIndex[3] = 0;
        for (int j = 0; j < roadSegments; j++)
        {
            float test = (float)j / (roadSegments - 1.0f);
            OrientedPoint op = GetBezierOn(test);
            verts[0] = op.LocalToWorld(Vector3.right * -1.5f);
            verts[1] = op.LocalToWorld(Vector3.right * 1.5f);
            verts[2] = op.LocalToWorld(-Vector3.up * 0.2f + Vector3.right * 1.0f);
            verts[3] = op.LocalToWorld(-Vector3.up * 0.2f + Vector3.right * -1.0f);
            for (int i = 0; i < vertsIndex.Length; i++)
            {
                Gizmos.DrawLine(verts[i], verts[vertsIndex[i]]);
                Gizmos.DrawSphere(verts[i], 0.08f);
            }
        }
    }
    private OrientedPoint GetBezierOn(float t)
    {
        Vector3[] bezierPoints = new Vector3[gameObject.transform.childCount];
        if (bezierPoints == null) throw new ArgumentNullException(nameof(bezierPoints));
        int upperBound = gameObject.transform.childCount;
        for (int i = 0; i < upperBound; i++)
        {
            bezierPoints[i] = gameObject.transform.GetChild(i).position;
        }
        Vector3 tangent = Vector3.zero;
        while (upperBound > 0)
        {
            for (int i = 0; i < upperBound - 1; i++)
            {
                bezierPoints[i] = Vector3.Lerp(bezierPoints[i], bezierPoints[i + 1], t);
            }
            if (upperBound == 3)
            {
                tangent = (bezierPoints[1] - bezierPoints[0]).normalized;
            }
            upperBound--;
        }
        return new OrientedPoint(bezierPoints[0], tangent);
    }
    private void CalculateDeCasteljouAlgorithm()
    {
        Vector3[] bezierPoints = new Vector3[gameObject.transform.childCount];
        if (bezierPoints == null) throw new ArgumentNullException(nameof(bezierPoints));
        
        int upperBound = gameObject.transform.childCount;
        for (int i = 0; i < upperBound; i++)
        {
            //bezierPoints[i] = _controlPoints[i].position;
        }
        while (upperBound > 0)
        {
            for (int i = 0; i < upperBound - 1; i++)
            {
                bezierPoints[i] = Vector3.Lerp(bezierPoints[i], bezierPoints[i + 1], T);
            }
            upperBound--;
        }

        sphere.transform.position = bezierPoints[0];
    }
}
