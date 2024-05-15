using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed;

    [Header("Use -1 for clockwise and 1 for anticlockwise")]
    public float direction;

    
    void Update()
    {
        transform.Rotate(0, direction * speed * Time.deltaTime,0 );
    }
}
