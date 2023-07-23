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
    [Range(2,128)]
    public int roadControls = 4;
    public bool automaticMove = true;
    public bool drawGizmos = true;

    [Range(0.1f, 0.5f)]
    public float cubeVelocity = 0.5f;
    public Material cubeMaterial;
    GameObject cubeObject;

    private int controlPointsCount = 4;
    private List<Transform> controlPoints = new List<Transform>();
       
    Mesh mesh;

    private void InitMesh()
    {
        mesh = new Mesh();
        mesh.name = "Procedural Road";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void Awake()
    {
        InitMesh();
    }

    void Start()
    {
        T = 0;
        controlPoints.Clear();
        controlPointsCount = gameObject.transform.childCount;
        for (int i = 0; i < controlPointsCount; i++)
        {
            controlPoints.Add(gameObject.transform.GetChild(i));
        }
        cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeObject.GetComponent<MeshRenderer>().material = cubeMaterial;
    }

    private void RenderMesh()
    {
        mesh.Clear();
        List<Vector3> verts = new List<Vector3>();
        for (int i = 0; i < roadControls; i++)
        {
            OrientedPoint op = GetBezierOn((float)i / (roadControls - 1.0f));
            verts.Add(op.LocalToWorld(Vector3.right * -1.5f));
            verts.Add(op.LocalToWorld(-Vector3.up * 0.2f + Vector3.right * -1.0f));
            verts.Add(op.LocalToWorld(-Vector3.up * 0.2f + Vector3.right * 1.0f));
            verts.Add(op.LocalToWorld(Vector3.right * 1.5f));
        }
        List<int> triangles = new List<int>();
        for (int i = 0; i < roadControls - 1; i++)
        {
            int root = i * 4;
            int rootNext = (i + 1) * 4;

            triangles.Add(root);
            triangles.Add(rootNext);
            triangles.Add(root + 3);
            triangles.Add(rootNext);
            triangles.Add(rootNext + 3);
            triangles.Add(root + 3);
            triangles.Add(root + 1);
            triangles.Add(rootNext);
            triangles.Add(root);
            triangles.Add(root + 1);
            triangles.Add(rootNext + 1);
            triangles.Add(rootNext);
            triangles.Add(root + 3);
            triangles.Add(rootNext + 3);
            triangles.Add(root + 2);
            triangles.Add(root + 2);
            triangles.Add(rootNext + 3);
            triangles.Add(rootNext + 2);
            triangles.Add(root + 1);
            triangles.Add(rootNext + 2);
            triangles.Add(rootNext + 1);
            triangles.Add(root + 1);
            triangles.Add(root + 2);
            triangles.Add(rootNext + 2);
        }
        mesh.SetVertices(verts);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
    }
    void Update() {
        RenderMesh();
        if (automaticMove)
        {
            T += Time.deltaTime * cubeVelocity;
            if (T > 1.0f) T = 0.0f;
        }
        cubeObject.transform.position = GetBezierOn(T).position;
        cubeObject.transform.position += Vector3.up * 0.5f;
        cubeObject.transform.rotation = GetBezierOn(T).rotation;
    }

    public void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        Gizmos.color = Color.magenta;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Gizmos.DrawSphere(gameObject.transform.GetChild(i).position, 0.05f);
        }
        Gizmos.color = Color.cyan;
        Vector3[] verts = new Vector3[4];
        int[] vertsIndex = new int[4] { 1,2,3,0};
        for (int j = 0; j < roadControls; j++)
        {
            OrientedPoint op = GetBezierOn((float)j / (roadControls - 1.0f));
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
}
