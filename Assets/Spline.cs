using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public Vector3[] splinePoint;
    public int splineCount;

    public bool debug_drawspline = true;
    void Start()
    {
        splineCount = transform.childCount;
        splinePoint = new Vector3[splineCount];

        for(int i = 0; i < splineCount; i++)
        {
            splinePoint[i] = transform.GetChild(i).position;
        }
    }

    
    void Update()
    {
        if (debug_drawspline)
        {
            if (splineCount > 1)
            {
                for (int i = 0; i < splineCount; i++)
                {
                    Debug.DrawLine(splinePoint[i], splinePoint[i + 1], Color.green);
                }
            }
        }
    }
}
