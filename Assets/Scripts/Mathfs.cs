using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mathfs
{
    public const float Tau = 6.2831853071f;
    public static Vector2 GetUnitVectorByAngle( float angRad)
    {
        return new Vector2(
            Mathf.Cos(angRad),
            Mathf.Sin(angRad)
        );
    }
}
