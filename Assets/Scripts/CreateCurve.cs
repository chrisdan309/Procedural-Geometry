using System;
using System.Collections.Generic;
using UnityEngine;

public class CreateCurve : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Transform> _controlPoints = new List<Transform>();
    [Range(0.0f, 1.0f)]
    public float t;
    public GameObject sphere;
    
    
    void Start()
    {
        
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _controlPoints.Add(gameObject.transform.GetChild(i));
        }

        sphere = Instantiate(sphere, _controlPoints[0].position, Quaternion.identity);
        t = 0;

    }

    // Update is called once per frame
    void Update()
    {

        CalculateDeCasteljouAlgorithm();
    }

    private void CalculateDeCasteljouAlgorithm()
    {
        Vector3[] bezierPoints = new Vector3[gameObject.transform.childCount];
        if (bezierPoints == null) throw new ArgumentNullException(nameof(bezierPoints));
        
        int upperBound = gameObject.transform.childCount;
        for (int i = 0; i < upperBound; i++)
        {
            bezierPoints[i] = _controlPoints[i].position;
        }
        while (upperBound > 0)
        {
            for (int i = 0; i < upperBound - 1; i++)
            {
                bezierPoints[i] = Vector3.Lerp(bezierPoints[i], bezierPoints[i + 1], t);
            }
            upperBound--;
        }

        sphere.transform.position = bezierPoints[0];
    }
}
