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

    [Range(3,256)]
    [SerializeField] int angularSegments = 3;
    
    private float radiusOuter => radiusInner + thickness;
    private float VertexCount => angularSegments * 2;

    private void OnDrawGizmosSelected()
    {
        Gizmosfs.DrawWireCircle(transform.position, transform.rotation,radiusInner, angularSegments);
        Gizmosfs.DrawWireCircle(transform.position, transform.rotation,radiusOuter, angularSegments);
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
