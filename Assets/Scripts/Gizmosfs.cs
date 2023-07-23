using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmosfs
{
    public static void DrawWireCircle(Vector3 pos, Quaternion rot, float radius, int detail = 32)
    {
        Vector3[] points3D = new Vector3[detail];
        for (int i = 0; i < detail; i++)
        {
            float t = i / (float )detail;
            float angRad = t * Mathfs.Tau;

            Vector2 point2D = Mathfs.GetUnitVectorByAngle(angRad) * radius;

            points3D[i] = pos +  rot * point2D;
        }

        for (int i = 0; i < detail; i++)
        {
            // Gizmos.DrawSphere(points3D[i], 0.02f);
            Gizmos.DrawLine(points3D[i], points3D[(i + 1) % detail]);

        }

    }
}
