using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadRing : MonoBehaviour
{
    [Range(0.01f, 2)]
    [SerializeField] float radiusInner;

    [Range(0.01f, 2)]
    [SerializeField] float thickness;

    private float radiusOuter => radiusInner + thickness;
    
    [Range(3,256)]
    [SerializeField] int angularSegments = 3;

    private const float TAU = 6.2831853071f;
    
    private void OnDrawGizmosSelected()
    {
        DrawWireCircle(transform.position, transform.rotation,1);
    }

    public void DrawWireCircle(Vector3 pos, Quaternion rot, float radius, int detail = 32)
    {
        Vector3[] points3D = new Vector3[detail];
        for (int i = 0; i < detail; i++)
        {
            float t = i / (float )detail;
            float angRad = t * TAU;

            Vector2 point2D = new Vector2(
                Mathf.Cos(angRad) * radius,
                Mathf.Sin(angRad) * radius
            );

            points3D[i] =pos +  rot * point2D;
        }

        for (int i = 0; i < detail; i++)
        {
            Gizmos.DrawSphere(points3D[i], 0.02f);
            
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
